using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class WinInfoTemplate : UserControl
  {
    public WinInfoTemplate() => InitializeComponent();

    public SmartTiler SmartTiler => ((SmartTiler)Tag);

    void onCloseSimilar(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
    }
    void onCloseByExe(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
      foreach (var w in SmartTiler.AllWindows.Where(r => r.ExePth.Contains(wi.ExePth)))
        Externs.CloseWindow(w.Handle);

      SmartTiler.CollectDesktopWindows();
    }
    void onCloseByTtl(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
      foreach (var w in SmartTiler.AllWindows.Where(r => r.WTitle.Contains(wi.WTitle)))
        Externs.CloseWindow(w.Handle);

      SmartTiler.CollectDesktopWindows();
    }
    void onCloseThisOne(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);

      Externs.CloseWindow(wi.Handle);
      SmartTiler.CollectDesktopWindows();
    }
    void onIgnoreExe(object s, RoutedEventArgs e) => SmartTiler.AddToIgnoreByExeName(((WindowInfo)((Button)s).Tag).ExePth);
    void onIgnoreTtl(object s, RoutedEventArgs e) => SmartTiler.AddToIgnoreByWiTitle(((WindowInfo)((Button)s).Tag).WTitle);
  }
}
