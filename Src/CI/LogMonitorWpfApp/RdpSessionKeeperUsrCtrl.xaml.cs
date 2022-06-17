namespace LogMonitorWpfApp;
public partial class RdpSessionKeeperUsrCtrl : UserControl
{
  readonly IdleTimeoutAnalizer _idleTimeoutAnalizer;
  readonly AppSettings _appset = AppSettings.Create();
  readonly Insomniac _insomniac = new();
  readonly string _crlf = $" ", TextLog = @$"RdpFacility.{Environment.MachineName}.Log.txt";
  readonly DateTime AppStarted = DateTime.Now;
  const int _from = 8, _till = 20, _dbgDelayMs = 500;
  int _dx = 10, _dy = 10, _hrsAdded = 0;
  bool _isLoaded = false;

  string Prefix => $"{DateTimeOffset.Now:HH:mm:ss}+{DateTimeOffset.Now - AppStarted:hh\\:mm\\:ss}  {(_appset.IsAudible ? "A" : "a")}{(_appset.IsPosning ? "P" : "p")}{(_appset.IsInsmnia ? "I" : "i")}  ";

  public RdpSessionKeeperUsrCtrl()
  {
    InitializeComponent();
    //Topmost = Debugger.IsAttached;
    var (ita, report) = IdleTimeoutAnalizer.Create(AppStarted);
    _idleTimeoutAnalizer = ita;
    tbkMin.Content = report;
    chkAdbl2.IsChecked = chkAdbl1.IsChecked = /*chkAdbl.IsChecked = */_appset.IsAudible;/*).Value;*/
    //chkInso2.IsChecked = chkInso1.IsChecked = /*chkInso.IsChecked = */_appset.IsInsmnia;/*).Value;*/
    chkPosn2.IsChecked = chkPosn1.IsChecked = /*chkPosn.IsChecked = */_appset.IsPosning;/*).Value;*/
    chkMind2.IsChecked = chkMind1.IsChecked = /*chkMind.IsChecked = */_appset.IsMindBiz;/*).Value;*/
  }
  async void OnLoaded(object s, RoutedEventArgs e)
  {
    //var v = new FileInfo(Environment.GetCommandLineArgs()[0]).LastWriteTime;
    await File.AppendAllTextAsync(TextLog, $"{AppStarted:yyyy-MM-dd}{_crlf}{Prefix}{(_idleTimeoutAnalizer.RanByTaskScheduler ? "+byTS" : "!byTS")} · args:{string.Join(' ', Environment.GetCommandLineArgs().Skip(1)),-12}  {_crlf}");
    if (_appset.IsInsmnia)
      _insomniac.RequestActive(_crlf);

    tbkMin.Content = $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(byTS)" : "(!byTS)")}";
    await Task.Delay(_dbgDelayMs);

    _ = new DispatcherTimer(TimeSpan.FromSeconds(_appset.PeriodSec), DispatcherPriority.Normal, new EventHandler(async (s, e) => await OnTick()), Dispatcher.CurrentDispatcher); //tu:

    if (_appset.IsAudible == true) Bpr.Tick();

    if (_idleTimeoutAnalizer.RanByTaskScheduler)
    {
      if (_idleTimeoutAnalizer.SkipLoggingOnSelf)
        _idleTimeoutAnalizer.SkipLoggingOnSelf = false;
      //else        EvLogHelper.LogScrSvrBgn(4 * 60, "RDP Facility - OnLoaded()  Idle Timeout ");
    }

    await OnTick();

    _isLoaded = true;
  }
  async Task OnTick(bool isManual = false)
  {
    if (_appset.IsAudible == true) Bpr.Tick();

    if (chkMind1.IsChecked == true)
    {
      var ibh = IsBizHours;
      //chkInso1.IsChecked = ibh;
      _insomniac.SetInso(ibh);
      Background = new SolidColorBrush(ibh ? Colors.DarkCyan : Colors.DarkRed);

      tbkHrs.Content = $"{_from} - {_till + _hrsAdded} : currently {(ibh ? "On" : "Off")}";

      if (!isManual && !ibh) _hrsAdded = 0; // reset to 0 for the next day.
    }

    if (_appset.IsPosning)
      TogglePosition("onTick");
    else
      await File.AppendAllTextAsync(TextLog, $"■"); // {prefix}onTick  {_crlf}");
  }

  bool IsBizHours => _from <= DateTimeOffset.Now.Hour && DateTimeOffset.Now.Hour <= (_till + _hrsAdded); // && DateTimeOffset.Now.Hour != 12 

  public IBpr Bpr { get; internal set; }

  void TogglePosition(string msg)
  {
    try
    {
      _idleTimeoutAnalizer.SkipLoggingOnSelf = true;
      _ = Mouse.Capture(this); // :fails outside of the owner window without it.
      var pointToScreen = PointToScreen(Mouse.GetPosition(this));

      _ = Win32.SetCursorPos((int)pointToScreen.X + _dx, (int)pointToScreen.Y + _dy);

      tbkLog.Text += $" {DateTime.Now:HHmm}:{pointToScreen} ";
    }
    catch (Exception ex) { _ = File.AppendAllTextAsync(TextLog, $"{Prefix}togglePosition  Exceptoin: {ex.Message}{_crlf}"); }
    finally
    {
      _ = Mouse.Capture(null);
      _dx = -_dx;
      _dy = -_dy;
      _ = File.AppendAllTextAsync(TextLog, $"{Prefix}tglPsn({msg}).{_crlf}");
    }
  }

  async void OnAudible(object s, RoutedEventArgs e) { _appset.IsAudible = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnInsmnia(object s, RoutedEventArgs e) { _appset.IsInsmnia = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); _insomniac.SetInso(((MenuItem)s).IsChecked == true); } }
  async void OnPosning(object s, RoutedEventArgs e) { _appset.IsPosning = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnMindBiz(object s, RoutedEventArgs e) { _appset.IsMindBiz = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnPlus1hr(object s, RoutedEventArgs e) { _hrsAdded++; await OnTick(true); Bpr.Tick(); }
  async void OnMinusHr(object s, RoutedEventArgs e) { _hrsAdded--; await OnTick(true); Bpr.Tick(); }

  void OnPosition(object s, RoutedEventArgs e) => TogglePosition("Manual Menu Call");

  async void OnMark(object z, RoutedEventArgs e) { var s = $"{Prefix}Mark     \t"; tbkLog.Text += s; await File.AppendAllTextAsync(TextLog, $"{s}{_crlf}"); }
  void OnRset(object s, RoutedEventArgs e) { _idleTimeoutAnalizer.MinTimeoutMin = 100; tbkMin.Content = $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(ro)" : "(RW)")}"; _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable(); }

  public async Task OnClosed(EventArgs e)
  {
    //if (_idleTimeoutAnalizer.RanByTaskScheduler && !_idleTimeoutAnalizer.SkipLoggingOnSelf)      EvLogHelper.LogScrSvrEnd(AppStarted.DateTime.AddSeconds(-4 * 60), 4 * 60, $"RDP Facility - OnClosed()  {DateTimeOffset.Now - AppStarted:hh\\:mm\\:ss}");

    _insomniac.RequestRelease(_crlf);
    await File.AppendAllTextAsync(TextLog, $"{Prefix}OnClosed   {DateTimeOffset.Now - AppStarted:hh\\:mm\\:ss}\n");
    if (_appset.IsAudible == true) Bpr.Tick();
    _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable();
  }
}