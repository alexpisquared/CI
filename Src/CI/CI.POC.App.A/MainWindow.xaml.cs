using System.Windows;

namespace CI.POC.App.A
{
  public partial class MainWindow : Window
  {
    public MainWindow() => InitializeComponent();

    void onClose(object sender, RoutedEventArgs e) => Close();
  }
}
