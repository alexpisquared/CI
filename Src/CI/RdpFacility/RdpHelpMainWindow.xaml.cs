﻿using RdpSessionKeeper;

namespace RdpFacility;

public partial class RdpHelpMainWindow : Window
{
  readonly IdleTimeoutAnalizer _idleTimeoutAnalizer;
  readonly AppSettings _appset = AppSettings.Create();
  readonly Insomniac _insomniac = new();
  readonly string _crlf = $" ";
  const int _from = 8, _till = 20, _dbgDelayMs = 500;
  int _dx = 10, _dy = 10, _hrsAdded = 0;
  bool _isLoaded = false;

  string Prefix => $"{DateTimeOffset.Now:HH:mm:ss}+{DateTimeOffset.Now - App.Started:hh\\:mm\\:ss}  {(_appset.IsAudible ? "A" : "a")}{(_appset.IsPosning ? "P" : "p")}{(_appset.IsInsmnia ? "I" : "i")}  ";

  public RdpHelpMainWindow()
  {
    InitializeComponent();
    Topmost = Debugger.IsAttached;
    MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
    var (ita, report) = IdleTimeoutAnalizer.Create(App.Started);
    _idleTimeoutAnalizer = ita;
    tbkMin.Content = report;
    chkAdbl2.IsChecked = chkAdbl1.IsChecked = /*chkAdbl.IsChecked = */_appset.IsAudible;/*).Value;*/
    //chkInso2.IsChecked = chkInso1.IsChecked = /*chkInso.IsChecked = */_appset.IsInsmnia;/*).Value;*/
    chkPosn2.IsChecked = chkPosn1.IsChecked = /*chkPosn.IsChecked = */_appset.IsPosning;/*).Value;*/
    chkMind2.IsChecked = chkMind1.IsChecked = /*chkMind.IsChecked = */_appset.IsMindBiz;/*).Value;*/

    if (_idleTimeoutAnalizer.RanByTaskScheduler)
    {
      WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

#if DEBUG //  if (Environment.MachineName == "RAZER1") { Top = 1700; Left = 1100; }
    if (Environment.MachineName == "RAZER1") { Top = 100; Left = 100; }
#endif
  }
  async void OnLoaded(object s, RoutedEventArgs e)
  {
    //var v = new FileInfo(Environment.GetCommandLineArgs()[0]).LastWriteTime;
    await File.AppendAllTextAsync(App.TextLog, $"{App.Started:yyyy-MM-dd}{_crlf}{Prefix}{(_idleTimeoutAnalizer.RanByTaskScheduler ? "+byTS" : "!byTS")} · args:{string.Join(' ', Environment.GetCommandLineArgs().Skip(1)),-12}  {_crlf}");
    if (_appset.IsInsmnia)
      _insomniac.RequestActive(_crlf);

    if (_idleTimeoutAnalizer.RanByTaskScheduler)
    {
      WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
    tbkMin.Content += $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(byTS)" : "(!byTS)")}";
    tbkBig.Content = Title = $"{(_appset.IsInsmnia ? "ON" : "Off")} @ {DateTimeOffset.Now:HH:mm}";
    await Task.Delay(_dbgDelayMs);

    _ = new DispatcherTimer(TimeSpan.FromSeconds(_appset.PeriodSec), DispatcherPriority.Normal, new EventHandler(async (s, e) => await OnTick()), Dispatcher.CurrentDispatcher); //tu:

    if (_appset.IsAudible == true) SystemSounds.Exclamation.Play();

    if (_idleTimeoutAnalizer.RanByTaskScheduler)
    {
      if (_idleTimeoutAnalizer.SkipLoggingOnSelf)
        _idleTimeoutAnalizer.SkipLoggingOnSelf = false;
      else
        EvLogHelper.LogScrSvrBgn(4 * 60, "RDP Facility - OnLoaded()  Idle Timeout ");
    }

    await OnTick();

    _isLoaded = true;
  }
  async Task OnTick(bool isManual = false)
  {
    if (chkMind1.IsChecked == true)
    {
      var ibh = IsBizHours;
      //chkInso1.IsChecked = ibh;
      _insomniac.SetInso(ibh);
      Background = new SolidColorBrush(ibh ? Colors.DarkCyan : Colors.DarkRed);

      tbkHrs.Content = $"{_from} - {_till + _hrsAdded} : currently {(ibh ? "On" : "Off")}";

      if (!isManual && !ibh) _hrsAdded = 0; // reset to 0 for the next day.
    }

    //if (_appset.IsPosning)
    //  TogglePosition("onTick");
    //else
    //  await File.AppendAllTextAsync(App.TextLog, $"■"); // {prefix}onTick  {_crlf}");
  }

  bool IsBizHours => _from <= DateTimeOffset.Now.Hour && DateTimeOffset.Now.Hour <= (_till + _hrsAdded); // && DateTimeOffset.Now.Hour != 12 

  void TogglePosition(string msg)
  {
    try
    {
      _idleTimeoutAnalizer.SkipLoggingOnSelf = true;
      Mouse.Capture(this);
      var pointToWindow = Mouse.GetPosition(this);
      var pointToScreen = PointToScreen(pointToWindow);
      var newPointToWin = new Win32.POINT((int)pointToWindow.X + _dx, (int)pointToWindow.Y + _dy);

      Win32.ClientToScreen(Process.GetCurrentProcess().MainWindowHandle, ref newPointToWin);

      //Jan 2022: nothing reliably works :(  must be the interferance of non-RDP screens
      //newPointToWin.x = 1400;  
      //newPointToWin.y = 1400;

      Win32.SetCursorPos(newPointToWin.x, newPointToWin.y);

      tbkLog.Text += $" {DateTime.Now:HHmm}:{pointToScreen} ";
      WriteLine($"** XY: {pointToWindow,12}  \t {pointToScreen,12} \t {newPointToWin.x,6:N0}-{newPointToWin.y,-6:N0}\t");
      if (_appset.IsAudible == true) SystemSounds.Asterisk.Play();
    }
    catch (Exception ex) { File.AppendAllTextAsync(App.TextLog, $"{Prefix}togglePosition  Exceptoin: {ex.Message}{_crlf}"); }
    finally
    {
      Mouse.Capture(null);
      _dx = -_dx;
      _dy = -_dy;
      File.AppendAllTextAsync(App.TextLog, $"{Prefix}tglPsn({msg}).{_crlf}");
    }
  }

  async void OnAudible(object s, RoutedEventArgs e) { _appset.IsAudible = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnInsmnia(object s, RoutedEventArgs e) { _appset.IsInsmnia = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); _insomniac.SetInso(((MenuItem)s).IsChecked == true); } }
  async void OnPosning(object s, RoutedEventArgs e) { _appset.IsPosning = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnMindBiz(object s, RoutedEventArgs e) { _appset.IsMindBiz = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnPlus1hr(object s, RoutedEventArgs e) { _hrsAdded++; await OnTick(true); SystemSounds.Exclamation.Play(); }
  async void OnMinusHr(object s, RoutedEventArgs e) { _hrsAdded--; await OnTick(true); SystemSounds.Exclamation.Play(); }

  void OnPosition(object s, RoutedEventArgs e) => TogglePosition("Manual Menu Call");

  async void OnMark(object z, RoutedEventArgs e) { var s = $"{Prefix}Mark     \t"; tbkLog.Text += s; await File.AppendAllTextAsync(App.TextLog, $"{s}{_crlf}"); }
  async void OnExit(object s, RoutedEventArgs e) { await File.AppendAllTextAsync(App.TextLog, $"{Prefix}onExit() by Escape.  {_crlf}"); Close(); }
  void OnRset(object s, RoutedEventArgs e) { _idleTimeoutAnalizer.MinTimeoutMin = 100; tbkMin.Content = $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(ro)" : "(RW)")}"; _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable(); }

  protected override async void OnClosed(EventArgs e)
  {
    if (_idleTimeoutAnalizer.RanByTaskScheduler && !_idleTimeoutAnalizer.SkipLoggingOnSelf)
      EvLogHelper.LogScrSvrEnd(App.Started.DateTime.AddSeconds(-4 * 60), 4 * 60, $"RDP Facility - OnClosed()  {DateTimeOffset.Now - App.Started:hh\\:mm\\:ss}");

    _insomniac.RequestRelease(_crlf);
    await File.AppendAllTextAsync(App.TextLog, $"{Prefix}OnClosed   {DateTimeOffset.Now - App.Started:hh\\:mm\\:ss}\n");
    base.OnClosed(e);
    if (_appset.IsAudible == true) SystemSounds.Hand.Play();
    _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable();
  }
}
