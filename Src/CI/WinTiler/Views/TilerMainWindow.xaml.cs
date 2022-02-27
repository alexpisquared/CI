using Ambience.Lib;
using CI.Standard.Lib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WinTiler.Lib;

namespace WinTiler.Views;

public partial class TilerMainWindow : CI.Visual.Lib.Base.WindowBase
{
  readonly SmartTiler _st = new();
  readonly ILogger<TilerMainWindow> _logger;
  readonly IConfigurationRoot? _config;
  DispatcherTimer _timer;
  DateTime _lastTime;
  Bpr _bpr = new Bpr();

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
  void onLoaded(object s, RoutedEventArgs e) { onBoth(s, e); }
  async void tick(object? s, EventArgs e)
  {
    var dt = (DateTime.Now - _lastTime);
    tbkTitl2.Content = $"_{(60 - dt.TotalSeconds):##}";
    if (dt.TotalSeconds > 60)
      await findWindows();
  }
  async Task findWindows()
  {
    _bpr.Beep(222, .150);
    ctrlPanel.Visibility = Visibility.Hidden; // ctrlPanel.IsEnabled = false; :too flashy
    Title = "Finding ...";
    _timer.Stop();
    try
    {
      //await Task.Delay(33);
      Title = _st.CollectDesktopWindows(chkSM.IsChecked == true); //         Task.Run(() => { var rv = _st.CollectDesktopWindows(); return rv; }).ContinueWith(_ => Title = _.Result, TaskScheduler.FromCurrentSynchronizationContext());
                                                                  //await Task.Delay(33);
      _lastTime = DateTime.Now;
    }
    catch (Exception ex) { _logger.LogError(ex, $""); /*ex.Pop(this);*/ }
    finally
    {
      ctrlPanel.Visibility = Visibility.Visible; // ctrlPanel.IsEnabled = true;
      _timer.Start();
      await _bpr.BeepAsync(333, .150);
    }
  }
#else
    void onLoaded(object s, RoutedEventArgs e)
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

  async void onFind(object s, RoutedEventArgs e) => await findWindows();
  async void onTile(object s, RoutedEventArgs e) { Title += " Tiling"; _st.Tile(chkPS.IsChecked); await _bpr.TickAsync(); }
  async void onBoth(object s, RoutedEventArgs e) { await findWindows(); onTile(s, e); }
  void onNotM(object s, RoutedEventArgs e) { _st.Tile(chkPS.IsChecked); ; }
  void onRstr(object s, RoutedEventArgs e) { }
  void onExit(object s, RoutedEventArgs e) { Close(); }

  internal async Task FindWindows()
  {
    if ((DateTime.Now - _lastTime).TotalMinutes >= 1)
      await findWindows();
    //else
    //  Title = $"Too early .. until {_lastTime.AddMinutes(1):HH:mm:ss}";
  }
}
