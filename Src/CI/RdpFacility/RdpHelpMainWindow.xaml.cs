using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace RdpFacility
{
  public partial class RdpHelpMainWindow : Window
  {
#if DEBUG
    const int _periodSec = 1, _till = 20, _dbgDelayMs = 2500;
    int _dx = 50, _dy = 50;
#else
    const int _periodSec = 60, _till = 20, _dbgDelayMs =0;
    int _dx = 1, _dy = 1;
#endif
    readonly Insomniac _dr = new Insomniac();
    DateTime _onSince;

    public RdpHelpMainWindow() { InitializeComponent(); SystemSounds.Hand.Play(); }
    void onTick()
    {
      if (DateTime.Now.Hour >= _till && checkBox.IsChecked == true)
        setDR(false);

      try
      {
        Mouse.Capture(this);
        var pointToWindow = Mouse.GetPosition(this);
        var pointToScreen = PointToScreen(pointToWindow);
        tbkBig.Text += $"{pointToScreen}  ";

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

    async void onLoaded(object s, RoutedEventArgs e)
    {
      await File.AppendAllTextAsync(App.TextLog, $"\n{App.Started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs())}\n");
      await Task.Delay(_dbgDelayMs);
      checkBox.IsChecked = true;
      _ = new DispatcherTimer(TimeSpan.FromSeconds(_periodSec), DispatcherPriority.Normal, new EventHandler((s, e) => onTick()), Dispatcher.CurrentDispatcher);
      SystemSounds.Exclamation.Play();
    }
    protected override void OnClosed(EventArgs e) /**/   { File.AppendAllText(App.TextLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {(DateTime.Now - App.Started):hh\\:mm\\:ss}  OnClosed  () \n"); base.OnClosed(e); SystemSounds.Hand.Play(); }

    void onStrt(object s, RoutedEventArgs e) => setDR(true);
    void onStop(object s, RoutedEventArgs e) => setDR(false);
    void onMove(object s, RoutedEventArgs e) { }
    void onMark(object sender, RoutedEventArgs e)
    {
      var s = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {(DateTime.Now - App.Started):hh\\:mm\\:ss}  Mark \n";
      tbkBig.Text += s;
      File.AppendAllText(App.TextLog, s);
    }

    void onExit(object sender, RoutedEventArgs e)
    {
      File.AppendAllText(App.TextLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {(DateTime.Now - App.Started):hh\\:mm\\:ss}  onExit()\n");

      Close();
    }


    void setDR(bool isOn)
    {
      if (isOn)
      {
        _dr.RequestActive();
        tbkLog.Text = $"{DateTime.Now:HH:mm:ss}\r\n";
      }
      else
      {
        _dr.RequestRelease();
      }

      tbkBig.Text = isOn ? $"On {(_onSince = DateTime.Now):HH:mm} ÷ {_till}:00" : $"Was On for {(DateTime.Now - _onSince):hh\\:mm}";
      Title = isOn ? $"{(_onSince = DateTime.Now):HH:mm}···" : $"Off";
    }
  }
}