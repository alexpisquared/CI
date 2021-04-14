using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class TilerMainWindow : CI.GUI.Support.WpfLibrary.Base.WindowBase
  {
    //public ObservableCollection<WindowInfo> _allWindows { get; set; } = new();
    readonly VirtDesktopMgr _vdm = new();
    readonly SmartTiler _st = new();

    public SmartTiler St => _st;

    public TilerMainWindow(Microsoft.Extensions.Logging.ILogger<TilerMainWindow> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot _config)
    {
      InitializeComponent();
      DataContext = this;
    }

    void onTile(object sender, RoutedEventArgs e) { _st.Tile(); ; }
    void onRestore(object sender, RoutedEventArgs e) { }

    void wnd_Loaded(object sender, RoutedEventArgs e) => collectDesktopWindows();


    void collectDesktopWindows()
    {
      DesktopWindowsStuff.GetDesktopWindowHandlesAndTitles(out var handles, out var titles, _vdm);

      Title = ($" ... Found  {titles.Count}  Windows of interest: ");

      _st.AllWindows.Clear();
      for (var i = 0; i < titles.Count; i++) _st.AllWindows.Add(new WindowInfo(titles[i], handles[i]));

      var c = 0;
      foreach (var w in _st.AllWindows.OrderBy(r => r.Sorter)) Console.WriteLine($"{++c,4}  {w}  ");
    }
  }
}
