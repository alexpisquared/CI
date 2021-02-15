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
    readonly IdleTimeoutAnalizer _idleTimeoutAnalizer;
#if DEBUG
    const int _periodSec = 1, _till = 20, _dbgDelayMs = 500;
    int _dx = 1, _dy = 1;
#else
    const int _periodSec = 60, _till = 20, _dbgDelayMs = 0;
    int _dx = 10, _dy = 10;
#endif
    readonly Insomniac _insomniac = new Insomniac();
    DateTimeOffset _onSince;

    public RdpHelpMainWindow()
    {
      InitializeComponent();
      var (ita, report) = IdleTimeoutAnalizer.LoadMe(App.Started);
      _idleTimeoutAnalizer = ita;
      tbkMin.Text = report;
      chkAdbl.IsChecked = _idleTimeoutAnalizer.IsAudible;
      chkInso.IsChecked = _idleTimeoutAnalizer.IsInsomnia;

      SystemSounds.Hand.Play();
    }
    async Task onTick()
    {
      if (DateTimeOffset.Now.Hour >= _till && chkInso.IsChecked == true)
        await setDR(false, false);

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
        if (_idleTimeoutAnalizer.IsAudible == true) SystemSounds.Asterisk.Play();
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
      await setDR(_idleTimeoutAnalizer.IsInsomnia == true, false);
      tbkMin.Text += $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.ModeRO ? "(ro)" : "(RW)")}";
      await File.AppendAllTextAsync(App.TextLog, $"\n{App.Started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs().Skip(1))}\t");
      await Task.Delay(_dbgDelayMs);
      _ = new DispatcherTimer(TimeSpan.FromSeconds(_periodSec), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
      SystemSounds.Exclamation.Play();
    }
    void onAuOn(object s, RoutedEventArgs e) => _idleTimeoutAnalizer.IsAudible = true;
    void onAuOf(object s, RoutedEventArgs e) => _idleTimeoutAnalizer.IsAudible = false;
    void onRset(object s, RoutedEventArgs e) => _idleTimeoutAnalizer.MinTimeoutMin = 100;
    async void onStrt(object s, RoutedEventArgs e) => await setDR(true);
    async void onStop(object s, RoutedEventArgs e) => await setDR(false);
    async void onMark(object z, RoutedEventArgs e) { var s = $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  Mark "; tbkLog.Text += s; await File.AppendAllTextAsync(App.TextLog, s); }
    async void onExit(object s, RoutedEventArgs e) { await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  onExit()\t"); Close(); }
    protected override async void OnClosed(EventArgs e)
    {
      await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  OnClosed\t");
      _insomniac.RequestRelease();
      base.OnClosed(e);
      if (_idleTimeoutAnalizer.IsAudible == true) SystemSounds.Hand.Play();
      _idleTimeoutAnalizer.SaveLastClose();
    }

    async Task setDR(bool isOn, bool preserveForLAter = true)
    {
      if (preserveForLAter) _idleTimeoutAnalizer.IsInsomnia = isOn;

      if (isOn)
      {
        _insomniac.RequestActive();
        tbkLog.Text = $"{DateTimeOffset.Now:HH:mm:ss}\r";
      }
      else
      {
        _insomniac.RequestRelease();
      }

      tbkBig.Text = isOn ? 
        $"ON  {(_onSince = DateTimeOffset.Now):HH:mm} ÷ {_till}:00" : 
        $"OFF  (was ON for {(DateTimeOffset.Now - _onSince):hh\\:mm\\:ss})";
      Title = isOn ? $"{(_onSince = DateTimeOffset.Now):HH:mm:ss} ···" : $"Off";
      await File.AppendAllTextAsync(App.TextLog, $"{tbkBig.Text}\t");
    }
  }
}