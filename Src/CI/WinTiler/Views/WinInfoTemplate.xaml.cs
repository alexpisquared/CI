using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class WinInfoTemplate : UserControl
  {
    public WinInfoTemplate() => InitializeComponent();

    public SmartTiler SmartTiler { get; set; }

    void onCloseSimilar(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
    }
    void onCloseByExe(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
      var st = ((SmartTiler)Tag);
      foreach (var w in st.AllWindows.Where(r => r.ExePth.Contains(wi.ExePth)))
        Externs.CloseWindow(w.Handle);

      st.CollectDesktopWindows();
    }
    void onCloseByTtl(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
      var st = ((SmartTiler)Tag);
      foreach (var w in st.AllWindows.Where(r => r.WTitle.Contains(wi.WTitle)))
        Externs.CloseWindow(w.Handle);

      st.CollectDesktopWindows();
    }
    void onCloseThisOne(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);

      Externs.CloseWindow(wi.Handle);
      ((SmartTiler)Tag)?.CollectDesktopWindows();
    }
  }
}
