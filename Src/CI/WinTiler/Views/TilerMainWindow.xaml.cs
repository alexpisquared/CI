using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class TilerMainWindow : CI.GUI.Support.WpfLibrary.Base.WindowBase
  {
    readonly VirtDesktopMgr _vdm = new();
    readonly SmartTiler _st = new();
    DateTime _lastTime;


    public SmartTiler St => _st;

    public TilerMainWindow(Microsoft.Extensions.Logging.ILogger<TilerMainWindow> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot _config)
    {
      InitializeComponent();
      DataContext = this;
    }

#if true
    async void onLoaded(object sender, RoutedEventArgs e)
    {
      await findWindows();
      _ = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Background, new EventHandler((s, e) => tick(s, e)), Dispatcher.CurrentDispatcher);//tu: one-line timer

    }

    async void tick(object s, EventArgs e)
    {
      var dt = (DateTime.Now - _lastTime);
      Title = $"  {(60 - dt.TotalSeconds):##}";
      if (dt.TotalSeconds > 60)
        await findWindows();
    }

    async Task findWindows()
    {
      ZommablePanel.IsEnabled = false;
      Title = "Finding...";
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

    async void onFind(object sender, RoutedEventArgs e) => await findWindows();
    async void onTile(object sender, RoutedEventArgs e) { Title = "Tileing..."; await Task.Delay(33); _st.Tile(); ; }
    async void onBoth(object sender, RoutedEventArgs e) { onFind(sender, e); onTile(sender, e); }
    async void onNotM(object sender, RoutedEventArgs e) { _st.Tile(); ; }
    async void onRstr(object sender, RoutedEventArgs e) { }

    internal async Task FindWindows()
    {
      if ((DateTime.Now - _lastTime).TotalMinutes < 1)
        Title = $"Too early .. until {_lastTime.AddMinutes(1):HH:mm:ss}";
      else
        await findWindows();
    }
  }
}
