using AsLink;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    readonly IdleTimeoutAnalizer _idleTimeoutAnalizer;
    readonly AppSettings _appSettings = AppSettings.Create();
#if DEBUG
    const int _periodSec = 5, _till = 20, _dbgDelayMs = 500;
    int _dx = 1, _dy = 1;
#else
    const int _periodSec = 60, _till = 20, _dbgDelayMs = 0;
    int _dx = 10, _dy = 10;
#endif
    readonly Insomniac _insomniac = new Insomniac();
    DateTimeOffset _onSince;
    bool _isLoaded = false;

    public RdpHelpMainWindow()
    {
      InitializeComponent();
      var (ita, report) = IdleTimeoutAnalizer.Create(App.Started);
      _idleTimeoutAnalizer = ita;
      tbkMin.Content = report;
      chkAdbl.IsChecked = _appSettings.IsAudible;
      chkInso.IsChecked = _appSettings.IsInsmnia;
      chkPosn.IsChecked = _appSettings.IsPosning;
    }
    async void onLoaded(object s, RoutedEventArgs e)
    {
      await setDR(_appSettings.IsInsmnia == true, false);
      tbkMin.Content += $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(byTS)" : "(!byTS)")}";
      await File.AppendAllTextAsync(App.TextLog, $"\n{App.Started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs().Skip(1))}  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "byTS" : "!byTS")}  \t");
      await Task.Delay(_dbgDelayMs);
      _ = new DispatcherTimer(TimeSpan.FromSeconds(_periodSec), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
      if (_appSettings.IsAudible == true) SystemSounds.Exclamation.Play();

      if (_idleTimeoutAnalizer.RanByTaskScheduler) EvLogHelper.LogScrSvrBgn(4 * 60, "RDP Facility - OnLoaded()  Idle Timeout ");

      _isLoaded = true;
    }
    async Task onTick()
    {
      if (DateTimeOffset.Now.Hour >= _till && chkInso.IsChecked == true)
        await setDR(false, false);

      if (_appSettings.IsPosning)
        togglePosition();
    }

    private void togglePosition()
    {
      try
      {
        Mouse.Capture(this);
        var pointToWindow = Mouse.GetPosition(this);
        var pointToScreen = PointToScreen(pointToWindow);
        var newPointToWin = new Win32.POINT((int)pointToWindow.X + _dx, (int)pointToWindow.Y + _dy);

        Win32.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref newPointToWin);
        Win32.SetCursorPos(newPointToWin.x, newPointToWin.y);

        tbkLog.Content += $"{pointToScreen}  ";
        Debug.WriteLine($"** XY: {pointToWindow,12}  \t {pointToScreen,12} \t {newPointToWin.x,6:N0}-{newPointToWin.y,-6:N0}\t");
        if (_appSettings.IsAudible == true) SystemSounds.Asterisk.Play();
      }
      finally
      {
        Mouse.Capture(null);
        _dx = -_dx;
        _dy = -_dy;
      }
    }

    async void onAuOn(object s, RoutedEventArgs e) { _appSettings.IsAudible = ((CheckBox)s).IsChecked == true; if (_isLoaded) await _appSettings.StoreAsync(); }
    async void onDaOn(object s, RoutedEventArgs e) { _appSettings.IsPosning = ((CheckBox)s).IsChecked == true; if (_isLoaded) await _appSettings.StoreAsync(); }
    async void onStrt(object s, RoutedEventArgs e) { _appSettings.IsInsmnia = ((CheckBox)s).IsChecked == true; if (_isLoaded) await _appSettings.StoreAsync(); await setDR(((CheckBox)s).IsChecked == true); }
    void onRset(object s, RoutedEventArgs e) { _idleTimeoutAnalizer.MinTimeoutMin = 100; tbkMin.Content = $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(ro)" : "(RW)")}"; _idleTimeoutAnalizer.SaveLastClose(); }
    async void onMark(object z, RoutedEventArgs e) { var s = $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  Mark     \t"; tbkLog.Content += s; await File.AppendAllTextAsync(App.TextLog, s); }
    async void onExit(object s, RoutedEventArgs e)    {      await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  onExit() by Escape.  \t"); Close();    }
    protected override async void OnClosed(EventArgs e)
    {
      if (_idleTimeoutAnalizer.RanByTaskScheduler)
        EvLogHelper.LogScrSvrEnd(App.Started.DateTime.AddSeconds(-4 * 60), 4 * 60, "RDP Facility - OnClosed()");

      await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  OnClosed   \t");
      _insomniac.RequestRelease();
      base.OnClosed(e);
      if (_appSettings.IsAudible == true) SystemSounds.Hand.Play();
      _idleTimeoutAnalizer.SaveLastClose();
    }

    async Task setDR(bool isOn, bool preserveForLAter = true)
    {
      if (isOn)
      {
        _insomniac.RequestActive();
        tbkLog.Content = $"{DateTimeOffset.Now:HH:mm:ss}\r";
      }
      else
      {
        _insomniac.RequestRelease();
      }

      tbkBig.Content = isOn ?
        $"ON  {(_onSince = DateTimeOffset.Now):HH:mm} ÷ {_till}:00" :
        $"OFF  (was ON for {(DateTimeOffset.Now - _onSince):hh\\:mm\\:ss})";
      Title = isOn ? $"{(_onSince = DateTimeOffset.Now):HH:mm:ss} ···" : $"Off";
      await File.AppendAllTextAsync(App.TextLog, $"{tbkBig.Content}    \t");
    }
  }
}