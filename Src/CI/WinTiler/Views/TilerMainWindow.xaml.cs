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

   async void onLoaded(object sender, RoutedEventArgs e)
    {
      await Task.Yield(); // .Delay(33);
      Title = _st.CollectDesktopWindows();//_st.Tile();

      //Task.Run(() => { var rv = _st.CollectDesktopWindows(); return rv; }).ContinueWith(_ => Title = _.Result, TaskScheduler.FromCurrentSynchronizationContext());
    }

    void onTile(object sender, RoutedEventArgs e) { _st.Tile(); ; }
    void onRestore(object sender, RoutedEventArgs e) { }
  }
}
