using CI.PermissionManager.Views;
using System.Windows;

namespace CI.PermissionManager
{
  public partial class App : Application
  {
    PAsUsersSelectorWindow __;
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      //new MainWindow().Show();
      //new AppPermPAsWindow().Show();
      //new UserPAsWindow().Show();
      __ = new PAsUsersSelectorWindow();
      __.Show();
    }

    void ToggleButton_Click(object s, RoutedEventArgs e)
    {
      __.Recalc(s);
    }
  }
}
