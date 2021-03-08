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
        readonly AsynchronousServer _svr = new();
        readonly Brush
          _c0 = new SolidColorBrush(Colors.Gray),
          _c1 = new SolidColorBrush(Colors.Red),
          _c2 = new SolidColorBrush(Colors.Blue);
        int _cnt = 0;

        public TesterMainWindow()
        {
            InitializeComponent();
            _ = new DispatcherTimer(TimeSpan.FromSeconds(.250), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
        }

        async void onLoaded(object s, RoutedEventArgs e) => await doJob(new AsynchronousClient(), dfltBtn.Tag?.ToString()?.Split(':') ?? new[] { "www", "123", "stringPoc" });
        async Task onTick()
        {
            onRR();
            await Task.Yield();
            rb.Fill = b1.IsEnabled ? (_cnt++) % 2 == 0 ? _c1 : _c2 : _c0;
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
        async void onMe(object s, RoutedEventArgs e) { var c = new AsynchronousClient(); await c.ConnectSendClose_formerStartClient(Dns.GetHostName(), 11000); tbkReportClt.Text += c.Report; }
        async void onRealHere(object s, RoutedEventArgs e) { var c = new AsynchronousClient(); await c.ConnectSendClose(Dns.GetHostName(), 11000, "alex.pigida", "stringPoc"); tbkReportClt.Text += c.Report; }
        async void onTag(object s, RoutedEventArgs e) => await doJob(new AsynchronousClient(), ((Button)s).Tag?.ToString()?.Split(':') ?? new[] { "www", "123", "stringPoc" });
        void onClear(object? s = null, RoutedEventArgs? e = null) { tbkReportSvr.Text = tbkReportClt.Text = ""; ; }

        async Task doJob(AsynchronousClient asc, string[] ipj) => tbkReportClt.Text += await asc.ConnectSendClose(ipj[0], int.Parse(ipj[1]), "alex.pigida", ipj[2]); void onRR(object? s = null, RoutedEventArgs? e = null) { tbkReportSvr.Text += _svr.Report; ; }
    }
}
