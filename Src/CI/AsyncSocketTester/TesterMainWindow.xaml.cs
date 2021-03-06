using AsyncSocketLib;
using CI.GUI.Support.WpfLibrary.Base;
using System;
using System.Media;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AsyncSocketTester
{
  public partial class TesterMainWindow : WindowBase
  {
    //DispatcherTimer __ = new DispatcherTimer(TimeSpan.FromSeconds(.250), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
    readonly AsynchronousServer _svr = new AsynchronousServer();
    int _cnt = 0;
    Brush
      c0 = new SolidColorBrush(Colors.Gray),
      c1 = new SolidColorBrush(Colors.Red),
      c2 = new SolidColorBrush(Colors.Blue);

    public TesterMainWindow()
    {
      InitializeComponent();
      _ = new DispatcherTimer(TimeSpan.FromSeconds(.250), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
    }

    async void onLoaded(object s, RoutedEventArgs e) { /*chkSvr.IsChecked = true;*/ await Task.Yield(); }
    async Task onTick()
    {
      onRR();
      await Task.Yield();
      rb.Fill = b1.IsEnabled ? (_cnt++) % 2 == 0 ? c1 : c2 : c0;
    }

    async void onToggleSvrListening(object s, RoutedEventArgs e)
    {
      if (((CheckBox)s).IsChecked == true)
      {
        tbkReportSvr.Text += "■■■ Turning server ON  ... ";
        b1.IsEnabled = true;
        await _svr.StartListening(Dns.GetHostName(), 11000, SystemSounds.Hand.Play);
        tbkReportSvr.Text += "Done ▀▄▀▄▀  ON\n";
      }
      else
      {
        tbkReportSvr.Text += "■■■ Turning server Off ... ";
        _svr.StopAndClose();
        b1.IsEnabled = false;
        tbkReportSvr.Text += "Done ▀▄▀▄▀  Off\n";
      }

      await Task.Yield();
    }

    void onMe(object s, RoutedEventArgs e) { var c = new AsynchronousClient(); c.ConnectSendClose_formerStartClient(Dns.GetHostName(), 11000); tbkReportClt.Text += c.Report; }
    void onRealHere(object s, RoutedEventArgs e) { var c = new AsynchronousClient(); c.ConnectSendClose(Dns.GetHostName(), 11000, "alex.pigida"); tbkReportClt.Text += c.Report; }
    void onRealReal(object s, RoutedEventArgs e) { var c = new AsynchronousClient(); c.ConnectSendClose("10.10.19.152", 6756, "alex.pigida"); tbkReportClt.Text += c.Report; }
    void onRealRea2(object s, RoutedEventArgs e) { var c = new AsynchronousClient(); c.ConnectSendClose("MTDEVTSAPP01.bbssecurities.com", 22225, "alex.pigida"); tbkReportClt.Text += c.Report; }
    void onRR(object s = null, RoutedEventArgs e = null)    {      tbkReportSvr.Text += _svr.Report;      ;    }

  }
}
