using CI.DS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CI.DS.Visual.Views
{
  public partial class MainView : CI.GUI.Support.WpfLibrary.Base.WindowBase
  {
    public MainView(Microsoft.Extensions.Logging.ILogger<MainView> _logger, Microsoft.Extensions.Configuration.IConfigurationRoot _config)
    {
      InitializeComponent(); ;

      DataContext = new MainVM(_logger, _config);
    }

    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(MainView), new PropertyMetadata(.0)); public double Blur { get => (double)GetValue(BlurProperty); set => SetValue(BlurProperty, value); }

    void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = WindowState.Normal; }
    void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = WindowState.Maximized; }
    async void onSave(object s, RoutedEventArgs e) { }
    async void onAudio(object s, RoutedEventArgs e) { }
    async void onFlush(object s, RoutedEventArgs e) { }
    async void onAddMe(object s, RoutedEventArgs e) { }
    async void cbxSrvr_SelectionChanged(object s, RoutedEventArgs e) { }
    async void onSettings(object s, RoutedEventArgs e) { }
    //void onExit(object s, RoutedEventArgs e) { Close(); ; }
  }
}
