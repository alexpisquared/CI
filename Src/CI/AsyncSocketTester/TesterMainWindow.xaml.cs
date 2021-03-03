using AsyncSocketLib;
using CI.GUI.Support.WpfLibrary.Base;
using System;
using System.Media;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AsyncSocketTester
{
  public partial class TesterMainWindow : WindowBase
  {
    //DispatcherTimer __ = new DispatcherTimer(TimeSpan.FromSeconds(.250), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
    readonly AsynchronousServer _svr = new AsynchronousServer();

    public TesterMainWindow()
    {
      InitializeComponent();
      _ = new DispatcherTimer(TimeSpan.FromSeconds(.250), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
    }

    async void onLoaded(object sender, RoutedEventArgs e) { chkSvr.IsChecked = true; await Task.Yield(); }
    async Task onTick() { onRR(); await Task.Yield(); }

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

    void onMe(object s, RoutedEventArgs e)
    {
      var c = new AsynchronousClient();
      c.StartClient(Dns.GetHostName(), 11000);
      tbkReportClt.Text = c.Report;
    }
    void onRealMe(object s, RoutedEventArgs e)
    {
      var c = new AsynchronousClient();
      c.StartClientReal(Dns.GetHostName(), 11000, "alex.pigida");

    }
    void onRealReal(object s, RoutedEventArgs e)
    {
      var c = new AsynchronousClient();
      c.StartClientReal("10.10.19.152", 6756, "alex.pigida");

    }
    void onRR(object s = null, RoutedEventArgs e = null)
    {
      tbkReportSvr.Text = _svr.Report;
      ;
    }

  }
}
