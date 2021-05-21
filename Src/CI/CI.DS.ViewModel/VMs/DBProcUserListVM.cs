using CI.DS.ViewModel.Commands;
using CI.Standard.Lib.Helpers;
using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CI.DS.ViewModel.VMs
{
  public class DBProcUserListVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly InventoryContext _context;
    readonly List<SpdUsr> _spds = new();
    string _searchString = "", _sqlConStr = "sql con str";

    ICollectionView _spcv; public ICollectionView SpdCollectionView { get => _spcv; set => SetProperty(ref _spcv, value); }

    public DBProcUserListVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;
      _context = new(SqlConStr = _config["SqlConStr"]);
      _spcv = CollectionViewSource.GetDefaultView(_spds); // redundant warning stopper only.

      UpdateViewCommand = new UpdateViewCommand(mainVM);//{ GestureKey = Key.F5, GestureModifier = ModifierKeys.None, MouseGesture = MouseAction.RightClick };

      Task.Run(async () => await loadAllSPs()).ContinueWith(_ =>
      {
        _.Result.ForEach(r => _spds.Add(r));
        SpdCollectionView = CollectionViewSource.GetDefaultView(_spds);
        SpdCollectionView.Filter = filterSPDs;
        SpdCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(SpdUsr.Schema)));
        SpdCollectionView.SortDescriptions.Add(new SortDescription(nameof(SpdUsr.UFName), ListSortDirection.Ascending));
        Bpr.Tick();
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    bool filterSPDs(object obj) => obj is SpdUsr && ((obj as SpdUsr)?.UFName.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase) ?? false);

    async Task<List<SpdUsr>> loadAllSPs()
    {
      List<SpdUsr> rv = new();
      var connection = _context.Database.GetDbConnection();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
          connection.Open();

        using DbCommand command = connection.CreateCommand();
        command.CommandText = _sql;

        using System.Data.Common.DbDataReader? reader = await command.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
          Debug.WriteLine("No rows found.");
        }
        else
        {
          while (reader.Read())
          {
            try
            {
              var vals = new object [reader.FieldCount];
              Debug.WriteLine($"Depth:{reader.Depth}   {reader.GetValues(vals)}: {string.Join('\t', vals)}");
              rv.Add(new SpdUsr(
                reader.GetString("Schema"),
                reader.GetString("SPName"),
                reader.IsDBNull("Parameters") ? "" : reader.GetString("Parameters"),
                reader.IsDBNull("Definition") ? "~DBNull (no access to definition)" : reader.GetString("Definition"),
                reader.GetInt32("HasExecPerm")));
            }
            catch (SqlNullValueException ex) { _logger.LogError(ex.ToString()); }
          }
        }

        reader.Close();
      }
      catch (SqlNullValueException ex) { _logger.LogError(ex.ToString()); }
      catch (Exception ex) { _logger.LogError(ex.ToString()); }
      finally { connection.Close(); }

      return rv;
    }

    SpdUsr? _selectSPD; public SpdUsr? SelectSPD { get => _selectSPD; set => SetProperty(ref _selectSPD, value); }
    public string SearchString { get => _searchString; set { SetProperty(ref _searchString, value); SpdCollectionView.Refresh(); } }
    public string SqlConStr { get => _sqlConStr; set { SetProperty(ref _sqlConStr, value); } }

    public ICommand UpdateViewCommand { get; set; }
    public IConfigurationRoot Config => _config;
    public ILogger Logger => _logger;

    const string _sql = @"
SELECT  
  schema_name(obj.schema_id)                    AS [Schema], 
  obj.name                                      AS SPName,  
  par.Parameters,
  has_perms_by_name(name, 'OBJECT', 'EXECUTE')  AS HasExecPerm, 
  mod.Definition
FROM   sys.objects      obj
  join sys.sql_modules  mod     on mod.object_id = obj.object_id
  cross apply (select p.name + ' ' + TYPE_NAME(p.user_type_id) + ' ' + CAST(isnull(p.max_length,8) AS nvarchar(4000)) + ' ' + CAST(isnull(p.precision,8) AS nvarchar(4000)) + ' ' + CAST(isnull(p.is_output,8) AS nvarchar(4000)) + ',' 
             from sys.parameters p
             where p.object_id = obj.object_id and p.parameter_id != 0 
             for xml path ('')) par (parameters)
WHERE obj.type in ('P', 'X') AND (has_perms_by_name(name, 'OBJECT', 'EXECUTE') = 1) AND mod.execute_as_principal_id IS NULL
ORDER BY SPName
";
  }
}