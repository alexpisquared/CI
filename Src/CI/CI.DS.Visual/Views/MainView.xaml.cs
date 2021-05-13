using CI.DS.ViewModel;
using CI.DS.ViewModel.VMs;
using System.Threading.Tasks;
using System.Windows;

namespace CI.DS.Visual.Views
{
  public partial class MainView : CI.Visual.Lib.Base.WindowBase
  {
    public MainView(Microsoft.Extensions.Logging.ILogger<MainView> logger, Microsoft.Extensions.Configuration.IConfigurationRoot config)
    {
      InitializeComponent();

      themeSelector.ThemeApplier = ApplyTheme;
      DataContext = new MainVM(logger, config);
    }

    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(MainView), new PropertyMetadata(.0)); public double Blur { get => (double)GetValue(BlurProperty); set => SetValue(BlurProperty, value); }

    void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = WindowState.Normal; }
    void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = WindowState.Maximized; }
    async void onSave(object s, RoutedEventArgs e) { await Task.Yield(); ; }
    async void onAudio(object s, RoutedEventArgs e) { await Task.Yield(); ; }
    async void onFlush(object s, RoutedEventArgs e) { await Task.Yield(); ; }
    async void onSettings(object s, RoutedEventArgs e) { await Task.Yield(); ; }
  }
}
