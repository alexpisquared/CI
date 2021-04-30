using System.Windows;

namespace EfStoredProcWpfApp.Views
{
  public partial class MainEfSpWindow : CI.GUI.Support.WpfLibrary.Base.WindowBase
  {
    public MainEfSpWindow(Microsoft.Extensions.Logging.ILogger<MainEfSpWindow> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot? _config) => InitializeComponent();

    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(MainEfSpWindow), new PropertyMetadata(.0)); public double Blur { get => (double)GetValue(BlurProperty); set => SetValue(BlurProperty, value); }

    void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = WindowState.Normal; }
    void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = WindowState.Maximized; }
    void onSave(object s, RoutedEventArgs e) { }
    void onAudio(object s, RoutedEventArgs e) { }
    void onFlush(object s, RoutedEventArgs e) { }
    void onAddMe(object s, RoutedEventArgs e) { }
    void cbxSrvr_SelectionChanged(object s, RoutedEventArgs e) { }
    void onSettings(object s, RoutedEventArgs e) { }
  }
}
