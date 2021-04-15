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

    void onLoaded(object sender, RoutedEventArgs e) => Title = _st.CollectDesktopWindows();//_st.Tile();
    void onTile(object sender, RoutedEventArgs e) { _st.Tile(); ; }
    void onRestore(object sender, RoutedEventArgs e) { }
  }
}
