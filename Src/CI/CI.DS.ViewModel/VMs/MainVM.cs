﻿using CI.DS.ViewModel.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CI.DS.ViewModel
{
  public class MainVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly UserPrefs _userPrefs;

    ObservableCollection<string> _sqlServers = new();
    string _sqlServer = "Unknown";

    public MainVM(ILogger logger, IConfigurationRoot config)
    {
      _logger = logger;
      _config = config;
      _selectedVM = new StoredProcListVM(logger, config, this);
      UpdateViewCommand = new UpdateViewCommand(this);

      _config["ServerList"].Split(" ").ToList().ForEach(r => _sqlServers.Add(r));
      _userPrefs = UserPrefs.Load<UserPrefs>();
      SqlServer = _userPrefs.SqlServer;

      Task.Run(async () => await Task.Delay(1000)).ContinueWith(_ => RunAnimation = true, TaskScheduler.FromCurrentSynchronizationContext());
      Task.Run(async () => await Task.Delay(2000)).ContinueWith(_ => RunAnimation = false, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public ObservableCollection<string> SqlServers { get => _sqlServers; set => SetProperty(ref _sqlServers, value, true); }

    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")]
    public string SqlServer
    {
      get => _sqlServer; set
      {
        SetProperty(ref _sqlServer, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */
        _userPrefs.SqlServer = value;
        UserPrefs.Save<UserPrefs>(_userPrefs);
      }
    }

    ObservableValidator _selectedVM; public ObservableValidator SelectedVM
    {
      get => _selectedVM; set
      {
        RunAnimation = true;
        Task.Run(async () => await Task.Delay(333)).ContinueWith(_ =>
        {
          SetProperty(ref _selectedVM, value);
          RunAnimation = false;
        }, TaskScheduler.FromCurrentSynchronizationContext());
      }
    }
    bool _RunAnimation = false; public bool RunAnimation { get => _RunAnimation; set => SetProperty(ref _RunAnimation, value); }

    public ICommand UpdateViewCommand { get; set; }
    public IConfigurationRoot Config { get => _config; }
    public ILogger Logger { get => _logger; }
  }
}
