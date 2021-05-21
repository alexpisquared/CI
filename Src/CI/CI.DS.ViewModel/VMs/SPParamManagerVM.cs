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

      Task<Dbprocess>.Run(async () =>
      {
        var sp = await _dbCnxt.Dbprocesses.FirstOrDefaultAsync(r => r.StoredProcName.Equals(spd.SPName, StringComparison.OrdinalIgnoreCase));
        if (sp is null) sp = await createNewDBProcessAndStoreToDB(spd);
        if (sp is null) throw new ArgumentNullException("Impossible!!!");

        return sp;
      }).ContinueWith(_ =>
      {
        try
        {
          Debug.WriteLine($"** New DBP id = {_.Result.Id}  ");
          //_.Result.ForEach(row => Dbprocesss.Add(row));
        }
        catch (Exception ex) { _logger.LogError(ex, "Really?"); }
      }, TaskScheduler.FromCurrentSynchronizationContext());
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

      var rowsSaved = await _dbCnxt.SaveChangesAsync();

      return await _dbCnxt.Dbprocesses.FirstOrDefaultAsync(r => r.StoredProcName.Equals(spd.SPName, StringComparison.OrdinalIgnoreCase));
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

    ObservableCollection<DbprocessParameter> _DbprocessParameters = new(); public ObservableCollection<DbprocessParameter> DbprocessParameters { get => _DbprocessParameters; set => SetProperty(ref _DbprocessParameters, value, true); }


    public ICommand UpdateViewCommand { get; set; }
  }
}
