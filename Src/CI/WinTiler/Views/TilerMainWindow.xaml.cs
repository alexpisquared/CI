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
    readonly SmartTiler _st = new();

    public SmartTiler St => _st;

    public TilerMainWindow(Microsoft.Extensions.Logging.ILogger<TilerMainWindow> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot _config)
    {
      InitializeComponent();
      DataContext = this;
    }

#if true
    async void onLoaded(object sender, RoutedEventArgs e)
    {
      ZommablePanel.IsEnabled = false;
      Title = "Loading...";
      try
      {
        await Task.Delay(33);
        Title = _st.CollectDesktopWindows(); //         Task.Run(() => { var rv = _st.CollectDesktopWindows(); return rv; }).ContinueWith(_ => Title = _.Result, TaskScheduler.FromCurrentSynchronizationContext());
      }
      finally
      {
        ZommablePanel.IsEnabled = true;
      }
    }
#else
    void onLoaded(object sender, RoutedEventArgs e)
    {
      ZommablePanel.IsEnabled = false;
      Title = "Loading...";
      try
      {
        //await Task.Delay(33);
        //Title = _st.CollectDesktopWindows(); //         Task.Run(() => { var rv = _st.CollectDesktopWindows(); return rv; }).ContinueWith(_ => Title = _.Result, TaskScheduler.FromCurrentSynchronizationContext());

        var context = TaskScheduler.FromCurrentSynchronizationContext();

        Title = "Starting...";

        // Start a task - this runs on the background thread...
        Task task = Task.Factory.StartNew(() =>
        {
          //title += cdw();
          Title += rnd();
          Title += fff();

          var token = Task.Factory.CancellationToken;
          Task.Factory.StartNew(() =>
          {
            Title += " past first ";
          }, token, TaskCreationOptions.None, context);

          Thread.Sleep(1000);
        })
          //;        task.Wait();        task
          .ContinueWith(_ => Title += $" ■", context); // More commonly, we'll continue a task with a new task on the UI thread, since this lets us update when our "work" completes.
      }
      finally
      {
        ZommablePanel.IsEnabled = true;
      }
    }
#endif

    string cdw() { var rv = _st.CollectDesktopWindows(); return rv; }
    string fff() { Thread.Sleep(1000); return " Slept "; }
    string rnd() { double j = 100; var rand = new Random(); for (var i = 0; i < 100000000; ++i) { j *= rand.NextDouble(); } return " Randm "; }

    void onFind(object sender, RoutedEventArgs e) { onLoaded(sender, e); ; }
    void onTile(object sender, RoutedEventArgs e) { _st.Tile(false); ; }
    void onNotM(object sender, RoutedEventArgs e) { _st.Tile(true); ; }
    void onBoth(object sender, RoutedEventArgs e) { onFind(sender, e); onTile(sender, e); }
    void onRstr(object sender, RoutedEventArgs e) { }
  }
}
