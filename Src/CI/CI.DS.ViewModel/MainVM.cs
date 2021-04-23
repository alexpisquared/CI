using CI.DS.ViewModel.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace CI.DS.ViewModel
{
  public class MainVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    ObservableValidator _selectedVM;

    public MainVM(Microsoft.Extensions.Logging.ILogger logger, Microsoft.Extensions.Configuration.IConfigurationRoot config)
    {
      _logger = logger;
      _config = config;
      _selectedVM = new DemoVM(logger, config);
      UpdateViewCommand = new UpdateViewCommand(this);
    }

    public ObservableValidator SelectedVM { get => _selectedVM; set => SetProperty(ref _selectedVM, value); }

    public ICommand UpdateViewCommand { get; set; }
    public IConfigurationRoot Config { get => _config; }
    public ILogger Logger { get => _logger; }
  }
}
