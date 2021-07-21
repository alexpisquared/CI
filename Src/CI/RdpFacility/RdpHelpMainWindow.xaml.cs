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
using System.Windows.Media;
using System.Windows.Threading;

namespace RdpFacility
{
  public partial class RdpHelpMainWindow : Window
  {
    readonly IdleTimeoutAnalizer _idleTimeoutAnalizer;
    readonly AppSettings _appset = AppSettings.Create();
    readonly Insomniac _insomniac = new();
    readonly string _crlf = $" ";
#if DEBUG
    const int _from = 8, _till = 19, _dbgDelayMs = 500;
    int _dx = 1, _dy = 1;
#else
        const int _from = 8, _till = 18, _dbgDelayMs = 0;
        int _dx = 10, _dy = 10;
#endif
    bool _isLoaded = false;

    string prefix => $"{DateTimeOffset.Now:HH:mm:ss}+{DateTimeOffset.Now - App.Started:hh\\:mm\\:ss}  {(_appset.IsAudible ? "A" : "a")}{(_appset.IsPosning ? "P" : "p")}{(_appset.IsInsmnia ? "I" : "i")}  ";


    public RdpHelpMainWindow()
    {
      InitializeComponent();
      var (ita, report) = IdleTimeoutAnalizer.Create(App.Started);
      _idleTimeoutAnalizer = ita;
      tbkMin.Content = report;
      chkAdbl.IsChecked = _appset.IsAudible;
      chkInso.IsChecked = _appset.IsInsmnia;
      chkPosn.IsChecked = _appset.IsPosning;
      chkMind.IsChecked = _appset.IsMindBiz;

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
      await File.AppendAllTextAsync(App.TextLog, $"{App.Started:yyyy-MM-dd}{_crlf}{prefix}{(_idleTimeoutAnalizer.RanByTaskScheduler ? "+byTS" : "!byTS")} · {v:M.d.H.m} · args:{string.Join(' ', Environment.GetCommandLineArgs().Skip(1)),-12}  {_crlf}");
      if (_appset.IsInsmnia)
        _insomniac.RequestActive(_crlf);

      if (_idleTimeoutAnalizer.RanByTaskScheduler)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //if (_appset.IsPosning)
        togglePosition("onLoaded");
      }
      tbkMin.Content += $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(byTS)" : "(!byTS)")}";
      tbkBig.Content = Title = $"DiReq: {(_appset.IsInsmnia ? "ON" : "Off")} @ {DateTimeOffset.Now:HH:mm:ss} · {v:M.d.H.m}";
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

      await onTick();

      _isLoaded = true;
    }
    async Task onTick()
    {
      if (chkMind.IsChecked == true)
      {
        var ibh = IsBizHours;
        chkInso.IsChecked = ibh;
        _insomniac.SetInso(ibh);
        Background = new SolidColorBrush(ibh ? Colors.DarkCyan : Colors.DarkRed);
      }

      if (_appset.IsPosning)
        togglePosition("onTick");
      else
        await File.AppendAllTextAsync(App.TextLog, $"■"); // {prefix}onTick  {_crlf}");
    }

    static bool IsBizHours => _from <= DateTimeOffset.Now.Hour
  //&& DateTimeOffset.Now.Hour != 12 
  && DateTimeOffset.Now.Hour <= _till;

    void togglePosition(string msg)
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
      catch (Exception ex) { File.AppendAllTextAsync(App.TextLog, $"{prefix}togglePosition  Exceptoin: {ex.Message}{_crlf}"); }
      finally
      {
        Mouse.Capture(null);
        _dx = -_dx;
        _dy = -_dy;
        File.AppendAllTextAsync(App.TextLog, $"{prefix}tglPsn({msg}).{_crlf}");
      }
    }

    async void onAudible(object s, RoutedEventArgs e) { _appset.IsAudible = ((CheckBox)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
    async void onInsmnia(object s, RoutedEventArgs e) { _appset.IsPosning = ((CheckBox)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); _insomniac.SetInso(((CheckBox)s).IsChecked == true); } }
    async void onPosning(object s, RoutedEventArgs e) { _appset.IsInsmnia = ((CheckBox)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
    async void onMindBiz(object s, RoutedEventArgs e) { _appset.IsMindBiz = ((CheckBox)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
    void onRset(object s, RoutedEventArgs e) { _idleTimeoutAnalizer.MinTimeoutMin = 100; tbkMin.Content = $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(ro)" : "(RW)")}"; _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable(); }
    async void onMark(object z, RoutedEventArgs e) { var s = $"{prefix}Mark     \t"; tbkLog.Content += s; await File.AppendAllTextAsync(App.TextLog, $"{s}{_crlf}"); }
    async void onExit(object s, RoutedEventArgs e) { await File.AppendAllTextAsync(App.TextLog, $"{prefix}onExit() by Escape.  {_crlf}"); Close(); }
    protected override async void OnClosed(EventArgs e)
    {
      if (_idleTimeoutAnalizer.RanByTaskScheduler && !_idleTimeoutAnalizer.SkipLoggingOnSelf)
        EvLogHelper.LogScrSvrEnd(App.Started.DateTime.AddSeconds(-4 * 60), 4 * 60, $"RDP Facility - OnClosed()  {DateTimeOffset.Now - App.Started:hh\\:mm\\:ss}");

      _insomniac.RequestRelease(_crlf);
      await File.AppendAllTextAsync(App.TextLog, $"{prefix}OnClosed   {DateTimeOffset.Now - App.Started:hh\\:mm\\:ss}\n");
      base.OnClosed(e);
      if (_appset.IsAudible == true) SystemSounds.Hand.Play();
      _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable();
    }

  }
}