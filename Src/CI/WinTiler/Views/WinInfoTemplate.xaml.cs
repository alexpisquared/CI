using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class WinInfoTemplate : UserControl
  {
    public WinInfoTemplate() => InitializeComponent();

    SmartTiler _smartTiler => (SmartTiler)Tag;

    void onCloseSimilar(object s, RoutedEventArgs e) { var wi = ((WindowInfo)((Button)s).Tag); ; }
    void onCloseByExe(object s, RoutedEventArgs e)
    {
      foreach (var w in _smartTiler.AllWindows.Where(r => r.AppNme.Contains(((WindowInfo)((Button)s).Tag).AppNme)))
        Externs.CloseWindow(w.Handle);

      _smartTiler.CollectDesktopWindows();
    }
    void onCloseByTtl(object s, RoutedEventArgs e)
    {
      foreach (var w in _smartTiler.AllWindows.Where(r => r.WTitle.Contains(((WindowInfo)((Button)s).Tag).WTitle)))
        Externs.CloseWindow(w.Handle);

      _smartTiler.CollectDesktopWindows();
    }
    void onMaximThisOne(object s, RoutedEventArgs e) => Externs.Maximize(((WindowInfo)((Button)s).Tag).Handle);
    void onRestrThisOne(object s, RoutedEventArgs e) => Externs.Restored(((WindowInfo)((Button)s).Tag).Handle);
    void onMinimThisOne(object s, RoutedEventArgs e) => Externs.Minimize(((WindowInfo)((Button)s).Tag).Handle);
    void onCloseThisOne(object s, RoutedEventArgs e) { Externs.CloseWindow(((WindowInfo)((Button)s).Tag).Handle); _smartTiler.CollectDesktopWindows(); }
    void onIgnoreExe(object s, RoutedEventArgs e) { _smartTiler.AddToIgnoreByExeName(((WindowInfo)((Button)s).Tag).AppNme); _smartTiler.CollectDesktopWindows(); }
    void onIgnoreTtl(object s, RoutedEventArgs e) { _smartTiler.AddToIgnoreByWiTitle(((WindowInfo)((Button)s).Tag).WTitle); _smartTiler.CollectDesktopWindows(); }
  }
}
