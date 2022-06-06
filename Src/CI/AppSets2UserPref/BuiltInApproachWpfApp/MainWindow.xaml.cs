using System;
using System.Windows;

namespace BuiltInApproachWpfApp;

public partial class MainWindow : Window
{
  public MainWindow()
  {
    InitializeComponent();

    tbk0.Text = $"\n Brand: \t{Properties.Settings.Default.BrandColor} \n\n My:   \t{Properties.Settings.Default.MyColor} \n\n last\t{Properties.Settings.Default.LastRan}\r\n                                    + {DateTime.Now - Properties.Settings.Default.LastRan:mm\\:ss}\r\n now \t{DateTime.Now}\r\n\r\n";

    Properties.Settings.Default.Setting2 =
    Properties.Settings.Default.MyColor = DateTime.Now.ToString();
    Properties.Settings.Default.LastRan = DateTime.Now;
    Properties.Settings.Default.Save();
  }

  void OnClose(object sender, RoutedEventArgs e) => Close();
}
