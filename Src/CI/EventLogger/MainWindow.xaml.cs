using System;
using System.IO;
using System.Windows;

namespace EventLogger
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      t1.Text = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs())} \n";
    }

    void onMark(object sender, RoutedEventArgs e)
    {
      var s = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  Mark \n";
      t1.Text += s;
      File.AppendAllText(App._textLog, s);
    }

    void onExit(object sender, RoutedEventArgs e)
    {
      File.AppendAllText(App._textLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  Closd manually\n");

      Close();
    }
  }
}
