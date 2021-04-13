using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace WinTiler
{
  public partial class TilerMainWindow : CI.GUI.Support.WpfLibrary.Base.WindowBase
  {
    public ObservableCollection<WindowInfo> _allWindows { get; set; } = new();
    readonly VirtDesktopMgr _vdm = new();

    public TilerMainWindow(Microsoft.Extensions.Logging.ILogger<TilerMainWindow> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot _config)
    {
      InitializeComponent();
      DataContext = this;
    }

    void Button_Click(object sender, RoutedEventArgs e)    {    }

     void wnd_Loaded(object sender, RoutedEventArgs e)
    {
      collectDesktopWindows();
    }


    void collectDesktopWindows()
    {
      DesktopWindowsStuff.GetDesktopWindowHandlesAndTitles(out var handles, out var titles, _vdm);

      Debug.WriteLine($" ... Found  {titles.Count}  Windows of interest: ");

      _allWindows.Clear();
      for (var i = 0; i < titles.Count; i++) _allWindows.Add(new WindowInfo(titles[i], handles[i]));

      var c = 0;
      foreach (var w in _allWindows.OrderBy(r => r.Sorter)) Console.WriteLine($"{++c,4}  {w}  ");
    }

  }
}
