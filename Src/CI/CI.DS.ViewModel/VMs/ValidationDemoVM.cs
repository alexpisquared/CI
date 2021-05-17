using CI.DS.ViewModel.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace CI.DS.ViewModel.VMs
{
  public class ValidationDemoVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    string _stringMayNotBeEmpty = "This may not be empty", _sqlconstr = "", _whereami = "";

    public ValidationDemoVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;

      UpdateViewCommand = new UpdateViewCommand(mainVM);//{ GestureKey = Key.F5, GestureModifier = ModifierKeys.None, MouseGesture = MouseAction.RightClick };

      ValidateAllProperties();

      SqlConStr = _config["SqlConStr"];
      WhereAmI = _config["WhereAmI"];
    }

    public ICommand UpdateViewCommand { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string StringMayNotBeEmpty { get => _stringMayNotBeEmpty; set => SetProperty(ref _stringMayNotBeEmpty, value, true); }
    public string SqlConStr { get => _sqlconstr; set => SetProperty(ref _sqlconstr, value, true); }
    public string WhereAmI { get => _whereami; set => SetProperty(ref _whereami, value, true); }
  }
}
