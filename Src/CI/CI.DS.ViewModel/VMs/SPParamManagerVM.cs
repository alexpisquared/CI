using CI.DS.ViewModel.Commands;
using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;

namespace CI.DS.ViewModel.VMs
{
  public class SPParamManagerVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly MainVM _mainVM;
    readonly InventoryContext _dbCnxt;

    public SPParamManagerVM(ILogger logger, IConfigurationRoot config, MainVM mainVM, SpdAdm spd)
    {
      _logger = logger;
      _config = config;
      _mainVM = mainVM;
      _dbCnxt = new(_config["SqlConStr"]);

      UpdateViewCommand = new UpdateViewCommand(mainVM);

      ValidateAllProperties();

      asyncLoad(spd);
    }

    void asyncLoad(SpdAdm spd) => Task.Run(async () => await getOrCreateSpd(spd)).ContinueWith(_ =>
    {
      try
      {
        Dbprocess = _.Result;
        Debug.WriteLine($"** DBP id = {Dbprocess.Id}, Param count = {Dbprocess.DbprocessParameters.Count}.  ");
        Dbprocess.DbprocessParameters.ToList().ForEach(row => DbprocessParameters.Add(row));
      }
      catch (Exception ex) { _logger.LogError(ex, "Really?"); }
    }, TaskScheduler.FromCurrentSynchronizationContext());
    async Task<Dbprocess> getOrCreateSpd(SpdAdm spd)
    {
      try
      {
        await _dbCnxt.Dbprocesses.LoadAsync();
        await _dbCnxt.DbprocessParameters.LoadAsync();

        var sp = _dbCnxt.Dbprocesses.Local.FirstOrDefault(r => spd.SPName.Equals(r.StoredProcName));
        if (sp is null) sp = await createNewDBProcessAndStoreToDB(spd);
        if (sp is null) throw new ArgumentNullException("Impossible!!!");

        return sp;
      }
      catch (Exception ex) { _logger.LogError(ex, "go figure..."); throw; }
    }
    async Task<Dbprocess> createNewDBProcessAndStoreToDB(SpdAdm spd)
    {
      var now = DateTime.Now;
      var ndp = new Dbprocess { Created = now, DbprocessName = spd.UFName, StoredProcName = spd.SPName, DbprocessParameters = new List<DbprocessParameter>() };

      spd.Parameters.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(param =>
      {
        var ps = param.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        ndp.DbprocessParameters.Add(
          new DbprocessParameter
          {
            Created = now,
            ParameterName = ps[0],
            ParameterType = ps[1],
            DefaultValue = defaultValue(ps[1]),
            UserVisible = true,
            IsReadOnly = false,
            IsOutput = ps[4] != "0",
            UserFriendlyName = ps[0].Replace("@p", "").Replace("@", "").ToSentence()
          });
      });

      var newDBP = _dbCnxt.Dbprocesses.Add(ndp);

      var rowsSaved = await _dbCnxt.SaveChangesAsync();
      Report = $"New SP  '{spd.SPName}'  with  {ndp.DbprocessParameters.Count}  parameters added to DB.\r\n(all  {rowsSaved}  rows saved)";

      return await _dbCnxt.Dbprocesses.FirstOrDefaultAsync(r => r.StoredProcName.Equals(spd.SPName));
    }
    string defaultValue(string dbTypeName)
    {
      switch (dbTypeName)
      {
        case "date":
        case "datetime": return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        case "int": return "0";
        default: return "Abc 123";
      }
    }

    public SpdAdm? SpdAdm { get; set; }

    public Dbprocess Dbprocess { get; set; }
    readonly ObservableCollection<DbprocessParameter> _dbprocessParameters = new(); public ObservableCollection<DbprocessParameter> DbprocessParameters { get => _dbprocessParameters; }


    public ICommand UpdateViewCommand { get; set; }

    ICommand saveToDB; public ICommand SaveToDB => saveToDB ??= new RelayCommand(PerformSaveToDB);
    void PerformSaveToDB()
    {
      var rowsSaved = _dbCnxt.SaveChanges();
      Report = $"{rowsSaved} rows saved";
    }

    string report; public string Report { get => report; set => SetProperty(ref report, value); }
  }
}
