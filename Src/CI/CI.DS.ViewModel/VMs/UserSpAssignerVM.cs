using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using CI.DS.ViewModel.Commands;
using System.Collections;

namespace CI.DS.ViewModel.VMs
{
  public class UserSpAssignerVM : ObservableValidator
  {
    public ILogger _logger;
    public IConfigurationRoot _config;
    public MainVM _mainVM;

    public UserSpAssignerVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;
      _mainVM = mainVM;
      UpdateViewCommand = new UpdateViewCommand(mainVM);//{ GestureKey = Key.F5, GestureModifier = ModifierKeys.None, MouseGesture = MouseAction.RightClick };
    }


    IEnumerable sqlServerList; public IEnumerable SqlServerList { get => sqlServerList; set => SetProperty(ref sqlServerList, value); }

    public ICommand UpdateViewCommand { get; set; }
    ICommand _togglePermission; public ICommand TogglePermission => _togglePermission ??= new RelayCommand(PerformTogglePermission); void PerformTogglePermission() { }
  }
}