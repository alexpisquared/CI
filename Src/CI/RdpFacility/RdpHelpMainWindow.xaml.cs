using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
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
    const int _periodSec = 60, _till = 20, _dbgDelayMs = 0;
    int _dx = 1, _dy = 1;
#endif
    readonly Insomniac _dr = new Insomniac();
    DateTime _onSince;

    public RdpHelpMainWindow() { InitializeComponent(); SystemSounds.Hand.Play(); }
    async Task onTick()
    {
      if (DateTime.Now.Hour >= _till && checkBox.IsChecked == true)
        await setDR(false);

      try
      {
        Mouse.Capture(this);
        var pointToWindow = Mouse.GetPosition(this);
        var pointToScreen = PointToScreen(pointToWindow);
        var newPointToWin = new Win32.POINT((int)pointToWindow.X + _dx, (int)pointToWindow.Y + _dy);

        Win32.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref newPointToWin);
        Win32.SetCursorPos(newPointToWin.x, newPointToWin.y);

        tbkLog.Text += $"{pointToScreen}  ";
        Debug.WriteLine($"** XY: {pointToWindow,12}  \t {pointToScreen,12} \t {newPointToWin.x,6:N0}-{newPointToWin.y,-6:N0}\t");
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
      await File.AppendAllTextAsync(App.TextLog, $"\n{App.Started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs().Skip(1))}\t");
      await Task.Delay(_dbgDelayMs);
      checkBox.IsChecked = true;
      _ = new DispatcherTimer(TimeSpan.FromSeconds(_periodSec), DispatcherPriority.Normal, new EventHandler((s, e) => onTick()), Dispatcher.CurrentDispatcher);
      SystemSounds.Exclamation.Play();
    }
    async void onStrt(object s, RoutedEventArgs e) => await setDR(true);
    async void onStop(object s, RoutedEventArgs e) => await setDR(false);
    async void onMark(object z, RoutedEventArgs e) { var s = $"{DateTime.Now:HH:mm:ss} {(DateTime.Now - App.Started):hh\\:mm\\:ss}  Mark "; tbkLog.Text += s; await File.AppendAllTextAsync(App.TextLog, s); }
    async void onExit(object s, RoutedEventArgs e) { await File.AppendAllTextAsync(App.TextLog, $"{DateTime.Now:HH:mm:ss} {(DateTime.Now - App.Started):hh\\:mm\\:ss}  onExit()\t"); Close(); }
    protected override async void OnClosed(EventArgs e) { await File.AppendAllTextAsync(App.TextLog, $"{DateTime.Now:HH:mm:ss} {(DateTime.Now - App.Started):hh\\:mm\\:ss}  OnClosed  \t"); base.OnClosed(e); SystemSounds.Hand.Play(); }

    async Task setDR(bool isOn)
    {
      if (isOn)
      {
        _dr.RequestActive();
        tbkLog.Text = $"{DateTime.Now:HH:mm:ss}\r";
      }
      else
      {
        _dr.RequestRelease();
      }

      tbkBig.Text = isOn ? $"On {(_onSince = DateTime.Now):HH:mm} ÷ {_till}:00" : $"Was On for {(DateTime.Now - _onSince):hh\\:mm}";
      Title = isOn ? $"{(_onSince = DateTime.Now):HH:mm:ss} ···" : $"Off";
      await Task.Yield();
    }
  }
}