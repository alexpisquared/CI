using CI.PermissionManager.Views;
using System.Windows;

namespace CI.PermissionManager
{
  public partial class App : Application
  {
    PAsUsersSelectorWindow? _mainWindow;
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      //new MainWindow().Show();
      //new AppPermPAsWindow().Show();
      //new UserPAsWindow().Show();
      _mainWindow = new PAsUsersSelectorWindow();
      _mainWindow.Show();
    }

    void onTogglePermission(object s, RoutedEventArgs e) => _mainWindow?.Recalc(s);
  }
}
