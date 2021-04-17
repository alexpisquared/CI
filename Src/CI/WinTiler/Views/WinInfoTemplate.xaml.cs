using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class WinInfoTemplate : UserControl
  {
    public WinInfoTemplate() => InitializeComponent();

    SmartTiler _smartTiler => (SmartTiler)Tag;

    async void onCloseByExe(object s, RoutedEventArgs e)
    {
      Visibility = Visibility.Collapsed; await Task.Delay(99);

      foreach (var w in _smartTiler.AllWindows.Where(r => r.AppNme.Contains(((WindowInfo)((Button)s).Tag).AppNme)))
        Externs.CloseWindow(w.Handle);

      _smartTiler.CollectDesktopWindows();
    }
    async void onCloseByTtl(object s, RoutedEventArgs e)
    {
      Visibility = Visibility.Collapsed; await Task.Delay(99);

      foreach (var w in _smartTiler.AllWindows.Where(r => r.WTitle.Contains(((WindowInfo)((Button)s).Tag).WTitle)))
        Externs.CloseWindow(w.Handle);

      _smartTiler.CollectDesktopWindows();
    }
    void onMaximThis(object s, RoutedEventArgs e) => Externs.Maximize(((WindowInfo)((Button)s).Tag).Handle);
    void onRestrThis(object s, RoutedEventArgs e) => Externs.Restored(((WindowInfo)((Button)s).Tag).Handle);
    void onMinimThis(object s, RoutedEventArgs e) => Externs.Minimize(((WindowInfo)((Button)s).Tag).Handle);
    async void onCloseThis(object s, RoutedEventArgs e) { Visibility = Visibility.Collapsed; await Task.Delay(99); Externs.CloseWindow(((WindowInfo)((Button)s).Tag).Handle); _smartTiler.CollectDesktopWindows(); }
    async void onIgnoreExe(object s, RoutedEventArgs e) { Visibility = Visibility.Collapsed; await Task.Delay(99); _smartTiler.AddToIgnoreByExeName(((WindowInfo)((Button)s).Tag).AppNme); _smartTiler.CollectDesktopWindows(); }
    async void onIgnoreTtl(object s, RoutedEventArgs e) { Visibility = Visibility.Collapsed; await Task.Delay(99); _smartTiler.AddToIgnoreByWiTitle(((WindowInfo)((Button)s).Tag).WTitle); _smartTiler.CollectDesktopWindows(); }
  }
}
