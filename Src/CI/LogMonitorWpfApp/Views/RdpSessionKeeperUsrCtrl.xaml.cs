namespace LogMonitorWpfApp;
public partial class RdpSessionKeeperUsrCtrl : UserControl
{
  readonly IdleTimeoutAnalizer _idleTimeoutAnalizer;
  readonly AppSettings _appset = AppSettings.Create();
  readonly Insomniac _insomniac = new();
  readonly string _crlf = $" ", TextLog = @$"RdpFacility.{Environment.MachineName}.Log.txt";
  readonly DateTime AppStarted = DateTime.Now;
  const int _from = 8, _till = 20, _dbgDelayMs = 500;
  const string _mmc = "Manual Menu Call";
  private int _dx = 11;
  private readonly int _dy = 10;
  private int _hrsAdded = 0;
  bool _isLoaded = false;
  IBpr? _bpr;
  private Point _previos;

  public RdpSessionKeeperUsrCtrl()
  {
    InitializeComponent();
    DataContext = this;

    var (ita, report) = IdleTimeoutAnalizer.Create(AppStarted);
    _idleTimeoutAnalizer = ita;
    tbkMin.Content = report;
    chkAdbl2.IsChecked = chkAdbl1.IsChecked = _appset.IsAudible;
    chkPstng.IsChecked = chkPstng.IsChecked = _appset.IsPosning;
    chkInsoB.IsChecked = chkInsoA.IsChecked = _appset.IsInsmnia;
    chkMind2.IsChecked = chkMind1.IsChecked = _appset.IsMindBiz;
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
    IsOn = !IsOn;

    if (_appset.IsAudible == true)
    {
      await Bpr.BeepAsync(160, .4); await Bpr.BeepAsync(16, .2);
    }

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
      await TogglePosition("onTick");
    else
      await File.AppendAllTextAsync(TextLog, $"■"); // {prefix}onTick  {_crlf}");
  }

  bool IsBizHours => _from <= DateTimeOffset.Now.Hour && DateTimeOffset.Now.Hour <= (_till + _hrsAdded); // && DateTimeOffset.Now.Hour != 12 

  public IBpr Bpr { get => _bpr ?? throw new ArgumentNullException("▄▀▄▀▄▀▄▀▄"); internal set => _bpr = value; }
  public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register("IsOn", typeof(bool), typeof(RdpSessionKeeperUsrCtrl)); public bool IsOn { get => (bool)GetValue(IsOnProperty); set => SetValue(IsOnProperty, value); }
  string Prefix => $"{DateTimeOffset.Now:HH:mm:ss}+{DateTimeOffset.Now - AppStarted:hh\\:mm\\:ss}  {(_appset.IsAudible ? "A" : "a")}{(_appset.IsPosning ? "P" : "p")}{(_appset.IsInsmnia ? "I" : "i")}  ";

  async Task TogglePosition(string msg)
  {
    if (_appset.IsAudible == true)
    {
      await Bpr.BeepAsync(260, .4); await Bpr.BeepAsync(16, 2.0);
      await Bpr.BeepAsync(160, .4); await Bpr.BeepAsync(16, 0.2);
      await Bpr.BeepAsync(160, .4); await Bpr.BeepAsync(16, 0.2);
      await Bpr.BeepAsync(140, .8); await Bpr.BeepAsync(16, 0.2);
      await Bpr.BeepAsync(400, .2);
    }

    try
    {
      _idleTimeoutAnalizer.SkipLoggingOnSelf = true;

      _ = Mouse.Capture(this); // :fails outside of the owner window without it.
      var current = PointToScreen(Mouse.GetPosition(this));
      if (current == _previos)
      {
        _previos.X = current.X + _dx;
        WriteLine(tbkMin.Content = $"Idle: {current.X}►{_previos.X}");
        _ = Win32.SetCursorPos((int)_previos.X, (int)_previos.Y);        //_ = Win32.SetCursorPos(/*910*/(int)current.X + _dx, /*336*/(int)current.Y + _dy);        //await Task.Delay(99);        //_previos = PointToScreen(Mouse.GetPosition(this));
        _dx = -_dx;
      }
      else // moved or 1st time:
      {
        WriteLine(tbkMin.Content = $"Busy");
        _previos = current;
      }
    }
    catch (Exception ex) { _ = File.AppendAllTextAsync(TextLog, $"{Prefix}togglePosition  Exceptoin: {ex.Message}{_crlf}"); }
    finally
    {
      _ = Mouse.Capture(null);
      _ = File.AppendAllTextAsync(TextLog, $"{Prefix}tglPsn({msg}).{_crlf}");
    }
  }

  async void OnAudible(object s, RoutedEventArgs e) { _appset.IsAudible = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnInsmnia(object s, RoutedEventArgs e) { _appset.IsInsmnia = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); _insomniac.SetInso(((MenuItem)s).IsChecked == true); } }
  async void OnPosning(object s, RoutedEventArgs e) { _appset.IsPosning = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnMindBiz(object s, RoutedEventArgs e) { _appset.IsMindBiz = ((MenuItem)s).IsChecked == true; if (_isLoaded) { await _appset.StoreAsync(); } }
  async void OnPlus2hr(object s, RoutedEventArgs e) { _hrsAdded += 2; await OnTick(true); await Bpr.ClickAsync(); }
  async void OnMinusHr(object s, RoutedEventArgs e) { _hrsAdded -= 1; await OnTick(true); await Bpr.ClickAsync(); }
  async void OnPosition(object s, RoutedEventArgs e) => await TogglePosition(_mmc);
  async void OnMark(object z, RoutedEventArgs e) { var s = $"{Prefix}Mark     \t"; tbkLog.Text += s; await File.AppendAllTextAsync(TextLog, $"{s}{_crlf}"); }
  void OnRset(object s, RoutedEventArgs e) { _idleTimeoutAnalizer.MinTimeoutMin = 100; tbkMin.Content = $"ITA so far  {_idleTimeoutAnalizer.MinTimeoutMin:N1} min  {(_idleTimeoutAnalizer.RanByTaskScheduler ? "(ro)" : "(RW)")}"; _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable(); }

  public async Task OnClosed(EventArgs e)
  {
    //if (_idleTimeoutAnalizer.RanByTaskScheduler && !_idleTimeoutAnalizer.SkipLoggingOnSelf)      EvLogHelper.LogScrSvrEnd(AppStarted.DateTime.AddSeconds(-4 * 60), 4 * 60, $"RDP Facility - OnClosed()  {DateTimeOffset.Now - AppStarted:hh\\:mm\\:ss}");

    _insomniac.RequestRelease(_crlf);
    await File.AppendAllTextAsync(TextLog, $"{Prefix}OnClosed   {DateTimeOffset.Now - AppStarted:hh\\:mm\\:ss}\n");
    _idleTimeoutAnalizer.SaveLastCloseAndAnalyzeIfMarkable();
  }
}