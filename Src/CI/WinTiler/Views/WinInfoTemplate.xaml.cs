using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class WinInfoTemplate : UserControl
  {
    public WinInfoTemplate()
    {
      InitializeComponent();
    }

    void onCloseSimilar(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
    }
    void onCloseByExe(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
    }
    void onCloseByTtl(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);
    }
    void onCloseThisOne(object s, RoutedEventArgs e)
    {
      var wi = ((WindowInfo)((Button)s).Tag);

      Externs.CloseWindow(wi.Handle);
    }
  }
}
