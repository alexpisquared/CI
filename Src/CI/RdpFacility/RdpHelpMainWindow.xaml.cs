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
    readonly AppSettings _appset = AppSettings.Create();
    readonly Insomniac _insomniac = new Insomniac();
#if DEBUG
    const int _till = 20, _dbgDelayMs = 500;
    int _dx = 1, _dy = 1;
#else
    const int _till = 20, _dbgDelayMs = 0;
    int _dx = 10, _dy = 10;
#endif
    bool _isLoaded = false;

    public RdpHelpMainWindow()
    {
      InitializeComponent();
      var (ita, report) = IdleTimeoutAnalizer.Create(App.Started);
      _idleTimeoutAnalizer = ita;
      tbkMin.Content = report;
      chkAdbl.IsChecked = _appset.IsAudible;
      chkInso.IsChecked = _appset.IsInsmnia;
      chkPosn.IsChecked = _appset.IsPosning;

      if (_idleTimeoutAnalizer.RanByTaskScheduler)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
      }

#if DEBUG
      if (Environment.MachineName == "RAZER1") { Top = 1700; Left = 1100; }
#endif
    }
    async void onLoaded(object s, RoutedEventArgs e)
    {
      var v = new FileInfo(Environment.GetCommandLineArgs()[0]).LastWriteTime;
      await File.AppendAllTextAsync(App.TextLog, $"{App.Started:yyyy-MM-dd}\n{App.Started:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "+byTS" : "!byTS")}  ·  {v:M.d.H.m}  ·  args:'{string.Join(' ', Environment.GetCommandLineArgs().Skip(1))}'  \n");
      setInsomniac(_appset.IsInsmnia);
      if (_idleTimeoutAnalizer.RanByTaskScheduler)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //if (_appset.IsPosning)
        togglePosition();
      }
      tbkMin.Content += $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(byTS)" : "(!byTS)")}";
      tbkBig.Content = Title = $"DiReq: {(_appset.IsInsmnia ? "ON" : "Off")} @ {DateTimeOffset.Now:HH:mm:ss}  ·  {v:M.d.H.m}";
      await Task.Delay(_dbgDelayMs);
      _ = new DispatcherTimer(TimeSpan.FromSeconds(_appset.PeriodSec), DispatcherPriority.Normal, new EventHandler(async (s, e) => await onTick()), Dispatcher.CurrentDispatcher); //tu:
      if (_appset.IsAudible == true) SystemSounds.Exclamation.Play();

      if (_idleTimeoutAnalizer.RanByTaskScheduler)
      {
        if (_idleTimeoutAnalizer.SkipLoggingOnSelf)
          _idleTimeoutAnalizer.SkipLoggingOnSelf = false;
        else
          EvLogHelper.LogScrSvrBgn(4 * 60, "RDP Facility - OnLoaded()  Idle Timeout ");
      }

      _isLoaded = true;
    }
    async Task onTick()
    {
      //if (DateTimeOffset.Now.Hour >= _till && chkInso.IsChecked == true)        await setDR(false, false);

      if (_appset.IsPosning)
        togglePosition();

      await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  onTick  A:{_appset.IsAudible,-5}  P:{_appset.IsPosning,-5}  I:{_appset.IsInsmnia,-5}\n");
    }

    void togglePosition()
    {
      try
      {
        _idleTimeoutAnalizer.SkipLoggingOnSelf = true;
        Mouse.Capture(this);
        var pointToWindow = Mouse.GetPosition(this);
        var pointToScreen = PointToScreen(pointToWindow);
        var newPointToWin = new Win32.POINT((int)pointToWindow.X + _dx, (int)pointToWindow.Y + _dy);

        Win32.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref newPointToWin);
        Win32.SetCursorPos(newPointToWin.x, newPointToWin.y);

        tbkLog.Content += $"{pointToScreen}  ";
        Debug.WriteLine($"** XY: {pointToWindow,12}  \t {pointToScreen,12} \t {newPointToWin.x,6:N0}-{newPointToWin.y,-6:N0}\t");
        if (_appset.IsAudible == true) SystemSounds.Asterisk.Play();
      }
      catch (Exception ex) { File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  onTick  Exceptoin: {ex.Message}\n"); }
      finally
      {
        Mouse.Capture(null);
        _dx = -_dx;
        _dy = -_dy;
        File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  togglePosition()  DONE.\n");
      }
    }

    async void onAudible(object s, RoutedEventArgs e) { _appset.IsAudible = ((CheckBox)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
    async void onInsmnia(object s, RoutedEventArgs e) { _appset.IsPosning = ((CheckBox)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
    async void onPosning(object s, RoutedEventArgs e) { _appset.IsInsmnia = ((CheckBox)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); setInsomniac(((CheckBox)s).IsChecked == true); } }
    void onRset(object s, RoutedEventArgs e) { _idleTimeoutAnalizer.MinTimeoutMin = 100; tbkMin.Content = $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(ro)" : "(RW)")}"; _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable(); }
    async void onMark(object z, RoutedEventArgs e) { var s = $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  Mark     \t"; tbkLog.Content += s; await File.AppendAllTextAsync(App.TextLog, $"{s}\n"); }
    async void onExit(object s, RoutedEventArgs e) { await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  onExit() by Escape.  \n"); Close(); }
    protected override async void OnClosed(EventArgs e)
    {
      if (_idleTimeoutAnalizer.RanByTaskScheduler && !_idleTimeoutAnalizer.SkipLoggingOnSelf)
        EvLogHelper.LogScrSvrEnd(App.Started.DateTime.AddSeconds(-4 * 60), 4 * 60, "RDP Facility - OnClosed()");

      _insomniac.RequestRelease();
      await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  OnClosed   \n");
      base.OnClosed(e);
      if (_appset.IsAudible == true) SystemSounds.Hand.Play();
      _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable();
    }

    void setInsomniac(bool isOn)
    {
      if (isOn)
      {
        _insomniac.RequestActive();
      }
      else
      {
        _insomniac.RequestRelease();
      }
    }
  }
}