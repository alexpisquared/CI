using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using CI.DS.ViewModel.Commands;

namespace CI.DS.ViewModel.VMs
{
  public class UserSpSelectorVM : ObservableValidator
  {
    public ILogger _logger;
    public IConfigurationRoot _config;
    public MainVM _mainVM;

    public UserSpSelectorVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;
      _mainVM = mainVM;
      UpdateViewCommand = new UpdateViewCommand(mainVM);//{ GestureKey = Key.F5, GestureModifier = ModifierKeys.None, MouseGesture = MouseAction.RightClick };
    }

    public ICommand UpdateViewCommand { get; set; }
  }
}