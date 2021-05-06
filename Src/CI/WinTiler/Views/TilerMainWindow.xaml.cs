using CI.Standard.Lib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WinTiler.Lib;

namespace WinTiler.Views
{
  public partial class TilerMainWindow : CI.Visual.Lib.Base.WindowBase
  {
    readonly SmartTiler _st = new();
    private readonly ILogger<TilerMainWindow> _logger;
    private readonly IConfigurationRoot? _config;
    DispatcherTimer _timer;
    DateTime _lastTime;

    public SmartTiler St => _st; // binding

    public TilerMainWindow(ILogger<TilerMainWindow> logger, IConfigurationRoot? config)
    {
      InitializeComponent();
      DataContext = this;
      _logger = logger;
      _config = config;
      _timer = new(TimeSpan.FromSeconds(1), DispatcherPriority.Background, new EventHandler((s, e) => tick(s, e)), Dispatcher.CurrentDispatcher);//tu: one-line timer
    }

#if true
    async void onLoaded(object s, RoutedEventArgs e)
    {
      await findWindows(); ;
    }
    async void tick(object? s, EventArgs e)
    {
      var dt = (DateTime.Now - _lastTime);
      tbkTitl2.Content = $"_{(60 - dt.TotalSeconds):##}";
      if (dt.TotalSeconds > 60)
        await findWindows();
    }
    async Task findWindows()
    {
      ctrlPanel.IsEnabled = false;
      Title = "Finding ...";
      _timer.Stop();
      try
      {
        await Task.Delay(33);
        Title = _st.CollectDesktopWindows(chkSM.IsChecked == true); //         Task.Run(() => { var rv = _st.CollectDesktopWindows(); return rv; }).ContinueWith(_ => Title = _.Result, TaskScheduler.FromCurrentSynchronizationContext());
        await Task.Delay(33);
        _lastTime = DateTime.Now;
      }
      catch (Exception ex) { _logger.LogError(ex, $""); ex.Pop(this); }
      finally
      {
        ctrlPanel.IsEnabled = true;
        _timer.Start();
      }
    }
#else
    void onLoaded(object sender, RoutedEventArgs e)
    {
      Bpr.Beep(9000, 100);
      var token = Task.Factory.CancellationToken;
      var context = TaskScheduler.FromCurrentSynchronizationContext();

      ctrlPanel.IsEnabled = false;
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
        ctrlPanel.IsEnabled = true;
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
    async void onTile(object sender, RoutedEventArgs e) { Title = "Tile-ing ..."; await Task.Delay(33); _st.Tile(); ; }
    async void onBoth(object sender, RoutedEventArgs e) { await findWindows(); onTile(sender, e); }
    void onNotM(object sender, RoutedEventArgs e) { _st.Tile(); ; }
    void onRstr(object sender, RoutedEventArgs e) { }

    internal async Task FindWindows()
    {
      if ((DateTime.Now - _lastTime).TotalMinutes < 1)
        Title = $"Too early .. until {_lastTime.AddMinutes(1):HH:mm:ss}";
      else
        await findWindows();
    }
  }
}
