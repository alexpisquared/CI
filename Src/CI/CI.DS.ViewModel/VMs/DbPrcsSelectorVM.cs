using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CI.DS.ViewModel.VMs
{
  public class DbPrcsSelectorVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    string _selectedDBP, _pKey;
    ObservableCollection<DbProcess> _dbProcesses = new();

    public DbPrcsSelectorVM(ILogger logger, IConfigurationRoot config)
    {
      _logger = logger;
      _config = config;

      //UpdateViewCommand = new UpdateViewCommand(this);

      _dbProcesses.Add(new DbProcess("Dbps", "Dbps ... or just me", "DbpsDbpsDbpsDbpsDbpsDbpsDbpsDbpsDbpsDbpsDbpsDbps"));
      _dbProcesses.Add(new DbProcess("Demo", "Validation Demo    ", "DemoDDemoDemoDemoDemoDemoDemoDemoDemoDemoDemoemo"));
      _dbProcesses.Add(new DbProcess("Acbg", "All Cash by Group  ", "All Cash by Group or similar"));
      _dbProcesses.Add(new DbProcess("Nthg01", "Nothing 1 ", "Nthg01Nthg01Nthg01Nthg01Nthg01Nthg01Nthg01Nthg01Nthg01Nthg01"));
      _dbProcesses.Add(new DbProcess("Nthg02", "Nothing 2 ", "Nthg02Nthg02Nthg02Nthg02Nthg02Nthg02Nthg02Nthg02Nthg02Nthg02"));
      _dbProcesses.Add(new DbProcess("Nthg03", "Nothing 3 ", "Nthg03Nthg03Nthg03Nthg03Nthg03Nthg03Nthg03Nthg03Nthg03Nthg03"));
      _dbProcesses.Add(new DbProcess("Nthg04", "Nothing 4 ", "Nthg04Nthg04Nthg04Nthg04Nthg04Nthg04Nthg04Nthg04Nthg04Nthg04"));
      _dbProcesses.Add(new DbProcess("Nthg05", "Nothing 5 ", "Nthg05Nthg05Nthg05Nthg05Nthg05Nthg05Nthg05Nthg05Nthg05Nthg05"));
      _dbProcesses.Add(new DbProcess("Nthg06", "Nothing 6 ", "Nthg06Nthg06Nthg06Nthg06Nthg06Nthg06Nthg06Nthg06Nthg06Nthg06"));
      _dbProcesses.Add(new DbProcess("Nthg07", "Nothing 7 ", "Nthg07Nthg07Nthg07Nthg07Nthg07Nthg07Nthg07Nthg07Nthg07Nthg07"));
      _dbProcesses.Add(new DbProcess("Nthg08", "Nothing 8 ", "Nthg08Nthg08Nthg08Nthg08Nthg08Nthg08Nthg08Nthg08Nthg08Nthg08"));
      _dbProcesses.Add(new DbProcess("Nthg09", "Nothing 9 ", "Nthg09Nthg09Nthg09Nthg09Nthg09Nthg09Nthg09Nthg09Nthg09Nthg09"));
      _dbProcesses.Add(new DbProcess("Nthg10", "Nothing 10", "Nthg10Nthg10Nthg10Nthg10Nthg10Nthg10Nthg10Nthg10Nthg10Nthg10"));
      _dbProcesses.Add(new DbProcess("Nthg11", "Nothing 11", "Nthg11Nthg11Nthg11Nthg11Nthg11Nthg11Nthg11Nthg11Nthg11Nthg11"));
      _dbProcesses.Add(new DbProcess("Nthg12", "Nothing 12", "Nthg12Nthg12Nthg12Nthg12Nthg12Nthg12Nthg12Nthg12Nthg12Nthg12"));
      _dbProcesses.Add(new DbProcess("Nthg13", "Nothing 13", "Nthg13Nthg13Nthg13Nthg13Nthg13Nthg13Nthg13Nthg13Nthg13Nthg13"));
      _dbProcesses.Add(new DbProcess("Nthg14", "Nothing 14", "Nthg14Nthg14Nthg14Nthg14Nthg14Nthg14Nthg14Nthg14Nthg14Nthg14"));
      _dbProcesses.Add(new DbProcess("Nthg15", "Nothing 15", "Nthg15Nthg15Nthg15Nthg15Nthg15Nthg15Nthg15Nthg15Nthg15Nthg15"));
      _dbProcesses.Add(new DbProcess("Nthg16", "Nothing 16", "Nthg16Nthg16Nthg16Nthg16Nthg16Nthg16Nthg16Nthg16Nthg16Nthg16"));
      _dbProcesses.Add(new DbProcess("Nthg17", "Nothing 17", "Nthg17Nthg17Nthg17Nthg17Nthg17Nthg17Nthg17Nthg17Nthg17Nthg17"));
      _dbProcesses.Add(new DbProcess("Nthg18", "Nothing 18", "Nthg18Nthg18Nthg18Nthg18Nthg18Nthg18Nthg18Nthg18Nthg18Nthg18"));
      _dbProcesses.Add(new DbProcess("Nthg19", "Nothing 19", "Nthg19Nthg19Nthg19Nthg19Nthg19Nthg19Nthg19Nthg19Nthg19Nthg19"));
      _dbProcesses.Add(new DbProcess("Nthg20", "Nothing 20", "Nthg20Nthg20Nthg20Nthg20Nthg20Nthg20Nthg20Nthg20Nthg20Nthg20"));
      _dbProcesses.Add(new DbProcess("Nthg21", "Nothing 21", "Nthg21Nthg21Nthg21Nthg21Nthg21Nthg21Nthg21Nthg21Nthg21Nthg21"));
      _dbProcesses.Add(new DbProcess("Nthg22", "Nothing 22", "Nthg22Nthg22Nthg22Nthg22Nthg22Nthg22Nthg22Nthg22Nthg22Nthg22"));
      _dbProcesses.Add(new DbProcess("Nthg23", "Nothing 23", "Nthg23Nthg23Nthg23Nthg23Nthg23Nthg23Nthg23Nthg23Nthg23Nthg23"));
      _dbProcesses.Add(new DbProcess("Nthg24", "Nothing 24", "Nthg24Nthg24Nthg24Nthg24Nthg24Nthg24Nthg24Nthg24Nthg24Nthg24"));
      _dbProcesses.Add(new DbProcess("Nthg25", "Nothing 25", "Nthg25Nthg25Nthg25Nthg25Nthg25Nthg25Nthg25Nthg25Nthg25Nthg25"));
      _dbProcesses.Add(new DbProcess("Nthg26", "Nothing 26", "Nthg26Nthg26Nthg26Nthg26Nthg26Nthg26Nthg26Nthg26Nthg26Nthg26"));
      _dbProcesses.Add(new DbProcess("Nthg27", "Nothing 27", "Nthg27Nthg27Nthg27Nthg27Nthg27Nthg27Nthg27Nthg27Nthg27Nthg27"));
      _dbProcesses.Add(new DbProcess("Nthg28", "Nothing 28", "Nthg28Nthg28Nthg28Nthg28Nthg28Nthg28Nthg28Nthg28Nthg28Nthg28"));
      _dbProcesses.Add(new DbProcess("Nthg29", "Nothing 29", "Nthg29Nthg29Nthg29Nthg29Nthg29Nthg29Nthg29Nthg29Nthg29Nthg29"));
      _dbProcesses.Add(new DbProcess("Nthg30", "Nothing 30", "Nthg30Nthg30Nthg30Nthg30Nthg30Nthg30Nthg30Nthg30Nthg30Nthg30"));
    }

    public ObservableCollection<DbProcess> DbProcesses { get => _dbProcesses; set => SetProperty(ref _dbProcesses, value, true); }

    public string PKey { get => _pKey; set => SetProperty(ref _pKey, value); }

    public ICommand UpdateViewCommand { get; set; }
    public IConfigurationRoot Config => _config;
    public ILogger Logger => _logger;
  }

  public class DbProcess
  {
    public DbProcess(string pKey, string name, string desc) => (PKey, Name, Desc) = (pKey, name, desc);

    public string PKey { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }

    public override string ToString() { return Name; }
  }
}