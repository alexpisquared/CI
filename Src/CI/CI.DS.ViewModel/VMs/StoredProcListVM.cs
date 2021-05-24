using CI.DS.ViewModel.Commands;
using CI.Standard.Lib.Extensions;
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
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;

namespace CI.DS.ViewModel.VMs
{
  public class StoredProcListVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly InventoryContext _dbx;
    readonly List<SpdAdm> _spdl = new();
    readonly List<Database> _dbll = new();
    string _searchString = "", _report = "sql con str", _sqlConStr = "sql con str";

    public StoredProcListVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;
      _dbx = new(SqlConStr = _config["SqlConStr"]);
      _spdv = CollectionViewSource.GetDefaultView(_spdl); // redundant warning stopper only.
      _dblv = CollectionViewSource.GetDefaultView(_dbll); // redundant warning stopper only.

      UpdateViewCommand = new UpdateViewCommand(mainVM);//{ GestureKey = Key.F5, GestureModifier = ModifierKeys.None, MouseGesture = MouseAction.RightClick };

      IsBusy = Visibility.Visible;

      var sw = Stopwatch.StartNew();

      Task.Run(() =>
      {
        var dbll = _dbx.Databases.ToList();
        var spdl = new List<SpdAdm>();

        dbll.Where(d => d.IsActive == true).ToList().ForEach(r => loadAllSPs(r).ForEach(m => spdl.Add(m))); // dbll.Where(d => d.IsActive == true).ToList().ForEach(async r => (await loadAllSPs(r)).ForEach(m => spdl.Add(m)));

        return (dbll, spdl);
      }).ContinueWith(_ =>
      {
        _.Result.dbll.ForEach(r => _dbll.Add(r));
        DblCollectionView = CollectionViewSource.GetDefaultView(_dbll);
        DblCollectionView.Filter = filterdbls;
        //DblCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Database.Name)));
        DblCollectionView.SortDescriptions.Add(new SortDescription(nameof(Database.Name), ListSortDirection.Ascending));

        _.Result.spdl.ForEach(r => _spdl.Add(r));
        SpdCollectionView = CollectionViewSource.GetDefaultView(_spdl);
        SpdCollectionView.Filter = filterSPDs;
        SpdCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(SpdAdm.Schema)));
        SpdCollectionView.SortDescriptions.Add(new SortDescription(nameof(SpdAdm.UFName), ListSortDirection.Ascending));

        Report = $"Loaded {_dbll.Count} DBs, {_spdl.Count} SPs in {sw.ElapsedMilliseconds:N0} ms.";
        Bpr.Tick();
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    bool filterSPDs(object obj) => obj is SpdAdm && ((obj as SpdAdm)?.SPName.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase) ?? false);
    bool filterdbls(object obj) => obj is Database && ((obj as Database)?.Name.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase) ?? false);

    List<SpdAdm> loadAllSPs(Database db) // async Task<List<SpdAdm>> loadAllSPs(Database r)
    {
      List<SpdAdm> rv = new();
      try
      {
        var audit = $"** WhereAmI:  {_config["WhereAmI"]}    {_config["SqlConStrSansDBS"]}  ";
        Debug.WriteLine(audit);

        var connectionString = string.Format(_config["SqlConStrSansDBS"], db.Name);

        using (Microsoft.Data.SqlClient.SqlConnection connection = new(connectionString))
        {
          if (connection.State.Equals(ConnectionState.Closed))
            connection.Open();

          using var command = connection.CreateCommand();
          command.CommandText = _sql;

          using var reader = /*await*/ command.ExecuteReader/*Async*/();
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
                var vals = new object[reader.FieldCount];
                //Debug.WriteLine($"Depth:{reader.Depth}   {reader.GetValues(vals)}: {string.Join('\t', vals)}");
                rv.Add(new SpdAdm(
                  db.Name,
                  reader.GetString("Schema"),
                  reader.GetString("SPName"),
                  reader.IsDBNull("Parameters") ? "" : reader.GetString("Parameters"),
                  reader.IsDBNull("Definition") ? "~DBNull (no access to definition)" : reader.GetString("Definition"),
                  reader.GetInt32("HasExecPerm")));
              }
              catch (SqlNullValueException ex) { ex.Log(); _logger.LogError(ex.ToString()); }
              catch (Exception ex) { ex.Log(); _logger.LogError(ex.ToString()); }
            }
          }

          reader.Close();
          connection.Close();
        }
      }
      catch (SqlNullValueException ex) { ex.Log(); _logger.LogError(ex.ToString()); }
      catch (Exception ex) { ex.Log(); _logger.LogError(ex.ToString()); }
      finally
      {
        IsBusy = Visibility.Collapsed;
      }
      return rv;
    }

    ICollectionView _spdv; public ICollectionView SpdCollectionView { get => _spdv; set => SetProperty(ref _spdv, value); }
    ICollectionView _dblv; public ICollectionView DblCollectionView { get => _dblv; set => SetProperty(ref _dblv, value); }
    Database? _selectDBL; public Database? SelectDBL { get => _selectDBL; set => SetProperty(ref _selectDBL, value); }
    SpdAdm? _selectSPD; public SpdAdm? SelectSPD { get => _selectSPD; set => SetProperty(ref _selectSPD, value); }
    public string SearchString { get => _searchString; set { SetProperty(ref _searchString, value); SpdCollectionView.Refresh(); } }
    public string SqlConStr { get => _sqlConStr; set => SetProperty(ref _sqlConStr, value); }
    public string Report { get => _report; set => SetProperty(ref _report, value); }
    Visibility _isBusy; public Visibility IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

    public ICommand UpdateViewCommand { get; set; }
    ICommand? _saveToDB; public ICommand SaveToDB => _saveToDB ??= new RelayCommand(performSaveToDB); void performSaveToDB() => Report = $"{_dbx.SaveChanges()} rows saved";

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