using CI.Standard.Lib.Extensions;
using CI.DS.ViewModel.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace CI.DS.ViewModel.VMs
{
  public class MainVM : ObservableValidator
  {
    readonly IConfigurationRoot _config;
    readonly ILogger _logger;
    readonly UserPrefs _userPrefs;

    ObservableCollection<string> _sqlServers = new();
    string _sqlServer = "Unknown";

    public MainVM(ILogger logger, IConfigurationRoot config)
    {
      _logger = logger;
      _config = config;
      _selectedVM = new StoredProcListVM(logger, config, this);
      UpdateViewCommand = new UpdateViewCommand(this);
      _resetDbCommand = null;

      _config["ServerList"].Split(" ").ToList().ForEach(r => _sqlServers.Add(r));
      _userPrefs = UserPrefs.Load<UserPrefs>();
      SqlServer = _userPrefs.SqlServer;
      _currentuser = $"{Environment.UserName}"; //todo: use AD to get proper name, auto add current user to DB. auto-add roles, if missing.
      _currentRole = "Admin";
    }

    public StoredProcDetail? StoredProcDetail { get; internal set; } = null;
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
        RunAnimation = value is not StoredProcListVM;
        Task.Run(async () => await Task.Delay(333)).ContinueWith(_ => { SetProperty(ref _selectedVM, value); }, TaskScheduler.FromCurrentSynchronizationContext());
      }
    }
    bool _runAnimation = false; public bool RunAnimation { get => _runAnimation; set => SetProperty(ref _runAnimation, value); }
    string _currentuser; public string CurrentUser { get => _currentuser; set => SetProperty(ref _currentuser, value); }
    string _currentRole; public string CurrentRole { get => _currentRole; set => SetProperty(ref _currentRole, value); }

    public ICommand UpdateViewCommand { get; set; }
    public IConfigurationRoot Config { get => _config; }
    public ILogger Logger { get => _logger; }

    ICommand? _resetDbCommand; public ICommand ResetDbCommand => _resetDbCommand ??= new RelayCommand(ResetDb); async void ResetDb()
    {
      using var dbx = new InventoryContext(_config["SqlConStr"]);
      try
      {
#if DEBUG
        dbx.Database.ExecuteSqlRaw("--SET FOREIGN_KEY_CHECKS=0;");

        var ra1 = dbx.Database.ExecuteSqlRaw("TRUNCATE TABLE dpl.[Process_UserAccess]");
        var ra2 = dbx.Database.ExecuteSqlRaw("TRUNCATE TABLE dpl.[Parameter]");

        dbx.Roles.RemoveRange(dbx.Roles.ToList());
        dbx.Databases.RemoveRange(dbx.Databases.ToList());
        dbx.Dbprocesses.RemoveRange(dbx.Dbprocesses.ToList());

        var now = DateTime.Now;
        await dbx.Roles.AddRangeAsync(new[] {
        new Role { Id = "A", RoleName = "Admin", RoleDescription = "Administrator [as per DB Reset]", Created = now },
        new Role { Id = "U", RoleName = "User", RoleDescription = "Regular end user", Created = now }});

        var rowsaffected = await dbx.SaveChangesAsync();

        var ra3 = dbx.Database.ExecuteSqlRaw("INSERT INTO dpl.[Database] (Name, Notes, IsActive) SELECT name, '** Initial load. ', 0 FROM sys.databases WHERE (database_id > 4) ORDER BY name");

        _logger.LogDebug($"Reset DB command has been performed by {_currentuser} resulting in {rowsaffected} rows affected.");
#else
      _logger.LogCritical($"Reset DB command has been attempted ...futilely   (courtesy of {_currentuser}).");
#endif
      }
      catch (Exception ex) { ex.Log(); _logger.LogError(ex, "Really?"); }
      finally { dbx.Database.ExecuteSqlRaw("-- SET FOREIGN_KEY_CHECKS=1;"); }
    }
  }
}
