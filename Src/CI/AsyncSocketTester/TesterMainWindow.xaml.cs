using System.Net;
using System.Net;
using CI.GUI.Support.WpfLibrary.Base;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AsyncSocketLib;
using System.Media;

namespace AsyncSocketTester
{
  public partial class TesterMainWindow : WindowBase
  {
    readonly AsynchronousServer _svr = new AsynchronousServer();

    public TesterMainWindow()
    {
      InitializeComponent();
    }

    async void CheckBox_Checked(object s, RoutedEventArgs e)
    {
      if (((CheckBox)s).IsChecked == true)
      {
        b1.IsEnabled = true;
        await _svr.StartListening(Dns.GetHostName(), 11000, SystemSounds.Hand.Play);
      }
      else
      {
        _svr.StopAndClose();
        b1.IsEnabled = false;
      }

      await Task.Yield();
    }

    void Button_Click(object s, RoutedEventArgs e)
    {
      new AsynchronousClient().StartClient(Dns.GetHostName(), 11000);

    }

    void Button_Click_1(object s, RoutedEventArgs e)
    {
      new AsynchronousClient().StartClientReal(Dns.GetHostName(), 11000, "alex.pigida");

    }

    void Button_Click_2(object s, RoutedEventArgs e)
    {

      new AsynchronousClient().StartClientReal("10.10.19.152", 6756, "alex.pigida");

    }

    void onRR(object s, RoutedEventArgs e)
    {
      tbkReport.Text = _svr.Report;
    }
  }
}
