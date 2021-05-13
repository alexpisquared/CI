using CI.DS.ViewModel.Commands;
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
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CI.DS.ViewModel.VMs
{
  public class StoredProcListVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly InventoryContext _context;
    readonly List<StoredProcDetail> _spds = new();
    string _SearchString = "";    

    ICollectionView _spcv; public ICollectionView SpdCollectionView { get => _spcv; set => SetProperty(ref _spcv, value); }

    public StoredProcListVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;
      _context = new(_config["SqlConStr"]);
      _spcv = CollectionViewSource.GetDefaultView(_spds); // redundant warning stopper only.

      UpdateViewCommand = new UpdateViewCommand(mainVM);

      Task.Run(async () => await loadAllSPs()).ContinueWith(_ =>
      {
        _.Result.ForEach(r => _spds.Add(r));
        SpdCollectionView = CollectionViewSource.GetDefaultView(_spds);
        SpdCollectionView.Filter = filterSPDs;
        SpdCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(StoredProcDetail.Schema)));
        SpdCollectionView.SortDescriptions.Add(new SortDescription(nameof(StoredProcDetail.UFName), ListSortDirection.Ascending));
        System.Media.SystemSounds.Hand.Play(); // Visual.Lib.Helpers.Bpr.Start();
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    bool filterSPDs(object obj) => obj is StoredProcDetail && ((obj as StoredProcDetail)?.UFName.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase) ?? false);

    async Task<List<StoredProcDetail>> loadAllSPs()
    {
      List<StoredProcDetail> rv = new();
      System.Data.Common.DbConnection? connection = _context.Database.GetDbConnection();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
        {
          connection.Open();
        }

        using System.Data.Common.DbCommand command = connection.CreateCommand();
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
            try { rv.Add(new StoredProcDetail(reader.GetString(0), reader.GetString(1), reader.GetString(2))); Debug.WriteLine($"{reader.GetString(0)}"); }
            catch (SqlNullValueException ex) { Debug.WriteLine($"  ■ ■ ■ {reader.GetString(0)}   {ex.Message}"); }
          }
        }

        reader.Close();
      }
      catch (SqlNullValueException ex) { _logger.LogError(ex.ToString()); }
      catch (Exception ex) { _logger.LogError(ex.ToString()); }
      finally { connection.Close(); }

      return rv;
    }

    StoredProcDetail? _selectSPD; public StoredProcDetail? SelectSPD { get => _selectSPD; set => SetProperty(ref _selectSPD, value); }
    public string SearchString
    {
      get => _SearchString; set
      {
        SetProperty(ref _SearchString, value);
        SpdCollectionView.Refresh();
      }
    }

    public ICommand UpdateViewCommand { get; set; }
    public IConfigurationRoot Config => _config;
    public ILogger Logger => _logger;

    const string _sql = @"
select  obj.name AS SPName,  substring(par.parameters, 0, len(par.parameters)) as Parameters,  
--mod.Definition,  
schema_name(obj.schema_id) AS [Schema]
from sys.objects      obj
--join sys.sql_modules  mod     on mod.object_id = obj.object_id
cross apply (select p.name + ' ' + TYPE_NAME(p.user_type_id) + ' ' + CAST(isnull(p.max_length,'888') AS nvarchar(4000)) + ' ' + CAST(isnull(p.precision,'888') AS nvarchar(4000)) + ',' 
             from sys.parameters p
             where p.object_id = obj.object_id and p.parameter_id != 0 
             for xml path ('')) par (parameters)
where obj.type in ('P', 'X')
order by SPName";
  }
}