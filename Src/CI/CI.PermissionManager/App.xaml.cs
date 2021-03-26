using CI.PermissionManager.Views;
using System.Windows;

namespace CI.PermissionManager
{
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      new MainWindow().Show();
      new AppPermPAsWindow().Show();
      new UserPAsWindow().Show();
    }
  }
}
