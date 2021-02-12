using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Runtime.InteropServices;
using RdpFacility;

namespace EventLogger
{
  public partial class EventLoggerMainWindow : Window
  {
#if DEBUG
    const int _periodSec = 1, _till = 20;
    int _dx = 50, _dy = 50;
#else
    const int _periodSec = 60, _till = 20;
    int _dx = 1, _dy = 1;
#endif
    public EventLoggerMainWindow()
    {
      InitializeComponent();

      SystemSounds.Asterisk.Play();
      Loaded += onLoaded;
    }

    async void onLoaded(object sender, RoutedEventArgs e)
    {
      await File.AppendAllTextAsync(App.TextLog, $"\n{App.Started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs())}\n");

      tbkBig.Text = $"{App.Started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs())} \n";

      await Task.Delay(1999);

      _ = new DispatcherTimer(TimeSpan.FromSeconds(_periodSec), DispatcherPriority.Normal, new EventHandler((s, e) => onTick()), Dispatcher.CurrentDispatcher);
      SystemSounds.Exclamation.Play();
    }
    void onTick()
    {
      //if (DateTime.Now.Hour >= _till && checkBox.IsChecked == true)
      //  setDR(false);

      try
      {
        Mouse.Capture(this);
        var pointToWindow = Mouse.GetPosition(this);
        var pointToScreen = PointToScreen(pointToWindow);
        tbkBig.Text = $"{pointToScreen}\n";

        var newPoint = new Win32.POINT((int)pointToWindow.X + _dx, (int)pointToWindow.Y + _dy);
        Win32.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref newPoint);
        Win32.SetCursorPos(newPoint.x, newPoint.y);

        Debug.WriteLine($"** XY: {pointToWindow,12}  \t {pointToScreen,12} \t {newPoint.x,6:N0}-{newPoint.y,-6:N0}");
        SystemSounds.Asterisk.Play();
      }
      finally
      {
        Mouse.Capture(null);
        _dx = -_dx;
        _dy = -_dy;
      }
    }

    void onMark(object sender, RoutedEventArgs e)
    {
      var s = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  Mark \n";
      tbkBig.Text += s;
      File.AppendAllText(App.TextLog, s);
    }

    void onExit(object sender, RoutedEventArgs e)
    {
      File.AppendAllText(App.TextLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  Closd manually\n");

      Close();
    }
  }
}
