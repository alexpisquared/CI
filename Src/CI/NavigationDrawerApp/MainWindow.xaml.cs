using System;
using System.Windows;
using System.Windows.Input;

namespace NavigationDrawerApp
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
      KeyDown += (s, e) => { if (e.Key == Key.Escape) { Close(); System.Diagnostics.Trace.WriteLine($"{DateTime.Now:yy.MM.dd HH:mm:ss.f} => ::>Application.Current.Shutdown();n"); Application.Current.Shutdown(); } };
    }

    void Button_Click(object sender, RoutedEventArgs e) { Close(); App.Current.Shutdown(); }
  }
}
