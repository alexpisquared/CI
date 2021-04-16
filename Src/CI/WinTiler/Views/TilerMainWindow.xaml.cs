using CI.GUI.Support.WpfLibrary.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class TilerMainWindow : CI.GUI.Support.WpfLibrary.Base.WindowBase
  {
    readonly VirtDesktopMgr _vdm = new();
    SmartTiler _st = new();
    DateTime _lastTime;

    public SmartTiler St => _st;

    public TilerMainWindow(Microsoft.Extensions.Logging.ILogger<TilerMainWindow> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot _config)
    {
      InitializeComponent();
      DataContext = this;
    }

#if true
    async void onLoaded(object sender, RoutedEventArgs e) { await collectWindows(); ; }

    async Task collectWindows()
    {
      ZommablePanel.IsEnabled = false;
      Title = "Loading...";
      try
      {
        await Task.Delay(33);
        Title = _st.CollectDesktopWindows(chkSM.IsChecked == true); //         Task.Run(() => { var rv = _st.CollectDesktopWindows(); return rv; }).ContinueWith(_ => Title = _.Result, TaskScheduler.FromCurrentSynchronizationContext());
        _lastTime = DateTime.Now;
      }
      finally
      {
        ZommablePanel.IsEnabled = true;
      }
    }
#else
    void onLoaded(object sender, RoutedEventArgs e)
    {
      Bpr.Beep(9000, 100);
      var token = Task.Factory.CancellationToken;
      var context = TaskScheduler.FromCurrentSynchronizationContext();

      ZommablePanel.IsEnabled = false;
      var ttl = "Loading...";
      try
      {
        var task = Task.Factory.StartNew<SmartTiler>(() =>
        {
          var st = trg();

          ttl += st.Report;

          return st;
        }).ContinueWith(_ =>
        {
          ttl += $" ■";
          Bpr.Beep(10000, 100); 
          Title = ttl;
          _st = _.Result;
        }, context); 
      }
      finally
      {
        ZommablePanel.IsEnabled = true;
        Bpr.Beep(5000, 100);
      }
    }

    static SmartTiler trg()
    {
      var st = new SmartTiler();
      var ttl = st.CollectDesktopWindows(true);
      return st;
    }
#endif

    void onFind(object sender, RoutedEventArgs e) { onLoaded(sender, e); ; }
    void onTile(object sender, RoutedEventArgs e) { _st.Tile(); ; }
    void onBoth(object sender, RoutedEventArgs e) { onFind(sender, e); onTile(sender, e); }
    void onNotM(object sender, RoutedEventArgs e) { _st.Tile(); ; }
    void onRstr(object sender, RoutedEventArgs e) { }

    internal void FindWindows()
    {
      if ((DateTime.Now - _lastTime).TotalMinutes > 1)
      {
        onLoaded(null, null);
      }
    }
  }
}
