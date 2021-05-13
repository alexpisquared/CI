using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CI.DS.ViewModel.VMs
{
  public class DemoVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    string _stringMayNotBeEmpty = "This may not be empty", _sqlconstr = "", _whereami = "";

    public DemoVM(ILogger logger, IConfigurationRoot config)
    {
      _logger = logger;
      _config = config;

      ValidateAllProperties();

      SqlConStr = _config["SqlConStr"];
      WhereAmI = _config["WhereAmI"];
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string StringMayNotBeEmpty { get => _stringMayNotBeEmpty; set => SetProperty(ref _stringMayNotBeEmpty, value, true); }
    public string SqlConStr { get => _sqlconstr; set => SetProperty(ref _sqlconstr, value, true); }
    public string WhereAmI { get => _whereami; set => SetProperty(ref _whereami, value, true); }
  }
}
