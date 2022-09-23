namespace OpenAI;
public partial class MainWindow : Window
{
  readonly TimeSpan _waitDuration;
  readonly IConfigurationRoot _config;
  readonly Bpr bpr;
  readonly TextSender _ts = new();
  CancellationTokenSource? _cts;
  string _prevClpbrd = "";
  bool isTimer_On = false;

  public MainWindow()
  {
    InitializeComponent();
    DataContext = this;
    _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build(); //var secretProvider = _config.Providers.First(); if (secretProvider.TryGet("WhereAmI", out var secretPass))  WriteLine(secretPass);else  WriteLine("Hello, World!");
    bpr = new Bpr();
    _waitDuration = (Resources["WaitDuration"] as Duration?)?.TimeSpan ?? TimeSpan.FromSeconds(20);
  }
  async void Window_Loaded(object s, RoutedEventArgs e)
  {
    EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); }));
    MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };

    cbxTeams.ItemsSource = new string[] {
      _config["Ttl"],
      "PBO-581 DB Process Launcher App | Microsoft Teams",
      "Oleksa Pigid | Microsoft Teams",
      "Microsoft Teams"};
    cbxTeams.SelectedIndex = 0;

    _ = tbxPrompt.Focus();

    //_ts.FindByProc(_config["Prc"], WinTitle);

    EnabledY = true;
    await BlockingTimer();
  }
  void DeblockingTimer() => Task.Run(async () => await BlockingTimer()); async Task BlockingTimer()
  {
    try
    {
      using PeriodicTimer tmr = new(_waitDuration);
      _cts = new CancellationTokenSource();
      while (_cts is not null && await tmr.WaitForNextTickAsync(_cts.Token))
      {
        if (IsTimer_On && Application.Current is not null)
        {
          if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread
            await OnTimer("UI");
          else
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(async () => { await OnTimer("bg"); }));
        }

        if (_cts?.Token.IsCancellationRequested == true) // Poll on this property if you have to do other cleanup before throwing.
        {
          WriteLine(tbkReport.Text = $"║   PeriodicTimer: -- CancellationRequested => Cancelling timer.");
          // Clean up here, then...
          _cts.Token.ThrowIfCancellationRequested();
        }
      }
    }
    catch (OperationCanceledException ex) { WriteLine(tbkReport.Text = $"║   PeriodicTimer: {ex.Message}"); }
    catch (Exception ex)             /**/ { WriteLine(tbkReport.Text = ex.Message); }
    finally { _cts?.Dispose(); _cts = null; }
  }
  async Task OnTimer(string thread)
  {
    if (!EnabledY) return;
    EnabledY = false;

    WriteLine($"\n>>> Starting on  '{thread}' ... ");

    IsWaiting = false;
    await AllDialogSteps();
    IsWaiting = true;
    EnabledY = true;
  }
  async Task AllDialogSteps()
  {
    var questn = await ScrapeTAsync();
    if (tbxPrompt.Text == questn)        /**/ { WriteLine(tbkReport.Text = $"Ignoring the same msg: '{TextSender.SafeLengthTrim(questn)}'."); return; }
    if (tbkAnswer.Text.Contains(questn)) /**/ { WriteLine(tbkReport.Text = $"Ignoring prev answer: '{TextSender.SafeLengthTrim(questn)}'."); return; }

    tbxPrompt.Text = questn;

    if (IsAutoQrAI)
      await QueryAiAsync(btnQryAI);
    else
      tbkAnswer.Text = tbxPrompt.Text;

    if (IsAutoAnsr)
      await TypeMsgAsync();
    else
      WriteLine(tbkReport.Text = "Auto type is OFF.");
  }
  async Task<string> ScrapeLastMsgFromteams(IntPtr winh)
  {
    var text = await _ts.GetTargetTextFromWindow(winh, bpr.Tick);
    var ary = text.Trim().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
    if (ary.Length == 0)
      return "Nothing found";

    for (var i = Math.Min(4, ary.Length + 1) - 1; i >= 1; i--) { WriteLine($"·· {ary.Length - i,5})   {ary[^i]}"); }    //TMI: hundreds of lines!!! for (var i = 0; i < ary.Length; i++) { WriteLine($".. {i,5})   {ary[i]}"); } 

    return ary[^1] == "has context menu" && ary.Length > 1 ? ary[^2] : ary[^1];
  }
  async void OnCheckClipboardForData()
  {
    const int minLen = 10;
    try
    {
      if (!Clipboard.ContainsText() || _prevClpbrd == Clipboard.GetText()) { WriteLine(tbkReport.Text = $"Same   [{DateTime.Now:ss}]"); bpr.Error(); return; }

      _prevClpbrd = Clipboard.GetText();
      if (_prevClpbrd.Length < minLen) { WriteLine(tbkReport.Text = $"Too Small"); bpr.Start(); bpr.Error(); return; }

      WriteLine(tbkReport.Text = $"Valid");
      tbxPrompt.Text = _prevClpbrd.Trim();

      if (IsAutoQrAI) await QueryAiAsync(btnQryAI);
      if (IsAutoAnsr) TypeMsg(btnQryAI, new RoutedEventArgs());

      bpr.Error();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
  }
  public bool IsTimer_On { get => isTimer_On; set { IsWaiting = isTimer_On = value; } }
  public bool IsAutoQrAI { get; set; } = false;
  public bool IsAutoAnsr { get; set; } = false;
  public static readonly DependencyProperty WinTitleProperty = DependencyProperty.Register("WinTitle", typeof(string), typeof(MainWindow)); public string WinTitle { get => (string)GetValue(WinTitleProperty); set => SetValue(WinTitleProperty, value); }
  public static readonly DependencyProperty EnabledYProperty = DependencyProperty.Register("EnabledY", typeof(bool), typeof(MainWindow)); public bool EnabledY { get => (bool)GetValue(EnabledYProperty); set => SetValue(EnabledYProperty, value); }
  public static readonly DependencyProperty IsWaitingProperty = DependencyProperty.Register("IsWaiting", typeof(bool), typeof(MainWindow)); public bool IsWaiting { get => (bool)GetValue(IsWaitingProperty); set => SetValue(IsWaitingProperty, value); }

  async Task<string> ScrapeTAsync()
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Scraping Teams' last msg..."); //menu1.IsEnabled = false;

    _ = Mouse.Capture(this);
    var current = PointToScreen(Mouse.GetPosition(this));

    IntPtr? winh;
    try
    {
      Hide();
      winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);

      var (right, bottom) = _ts.GetRB(winh ?? throw new ArgumentNullException(nameof(winh)));
      await MouseOperations.MouseClickEventAsync(right - 32, bottom - 400);
      MouseOperations.SetCursorPosition((int)current.X, (int)current.Y);
      Show();
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (IndexOutOfRangeException ex)/**/{ WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (Exception ex)               /**/{ WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }

    try
    {
      var text = await ScrapeLastMsgFromteams(winh ?? throw new ArgumentNullException(nameof(winh), $"Window '{WinTitle}' not found"));
      return text;
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (IndexOutOfRangeException ex)/**/{ WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (Exception ex)               /**/{ WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    finally
    {
      Show();
      bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; //menu1.IsEnabled = true;

      _ = Mouse.Capture(null);
    }
  }
  async Task TypeMsgAsync()
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Typing into Teams..."); tbkAnswer.Background = Brushes.DarkRed;

    if (!Mouse.Capture(this)) throw new InvalidOperationException("Failed to get the WinH coordinates.");

    var rawPosn = Mouse.GetPosition(this);
    var ptsPosn = PointToScreen(rawPosn); WriteLine($"\t raw:{rawPosn}\t pts:{ptsPosn}  :before");

    try
    {
      var winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);
      if (winh is null)
        tbkReport.Text = $"Window '{WinTitle}' not found";
      else
      {
        Hide();
        var (right, bottom) = _ts.GetRB(winh ?? throw new ArgumentNullException(nameof(winh)));
        await MouseOperations.MouseClickEventAsync(right - 520, bottom - 88);
        MouseOperations.SetCursorPosition((int)ptsPosn.X, (int)ptsPosn.Y);
        Show();
        var failReport = _ts.SendMsg(winh ?? throw new ArgumentNullException(nameof(winh)), $"{tbkAnswer.Text}{{ENTER}}");
        if (failReport.Length > 0)
        {
          WriteLine(tbkReport.Text = $"FAILED SendKey(): {failReport}");
          return;
        }
      }
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally
    {
      _ = Mouse.Capture(null);
      Show();
      bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; tbkAnswer.Background = Brushes.Transparent;
    }
  }
  async Task QueryAiAsync(object s)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Asking AI ..."); tbkAnswer.Background = Brushes.DarkOliveGreen; ((Control)s).Visibility = Visibility.Hidden;
    try
    {
      tbkAnswer.Text = "Asking AI ...";

      (bt ??= new(TimeSpan.FromSeconds(.1))).Start(UpdateClock);
      started = DateTime.Now;

      var (ts, finishReason, answer) = await OpenAILib.OpenAI.CallOpenAI(_config, tbkMax.Text, tbxPrompt.Text);

      tbkAnswer.Text = answer.StartsWith("\n") ? answer.Trim('\n') : $"{tbxPrompt.Text}{answer}";
      tbkTM.Text = $"{ts.TotalSeconds:N1}";
      tbkFR.Text = finishReason;
      tbkLn.Text = $"{answer.Length}";
      tbkZZ.Text = "·";
    }
    finally
    {
      bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; tbkAnswer.Background = Brushes.Transparent; ((Control)s).Visibility = Visibility.Visible;
      if (bt is not null) await bt.StopAsync();
    }
  }
  async void QueryAI(object s, RoutedEventArgs e) => await QueryAiAsync(s);
  async void SetText(object s, RoutedEventArgs e) { bpr.Start(); _prevClpbrd = tbkAnswer.Text; Clipboard.SetText(tbkAnswer.Text); await Task.Yield(); }
  async void ScrapeT(object s, RoutedEventArgs e) => tbxPrompt.Text = await ScrapeTAsync();
  async void TypeMsg(object s, RoutedEventArgs e) => await TypeMsgAsync();
  async void TabNSee(object s, RoutedEventArgs e)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "TabNSee()...");

    try
    {
      var winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);
      if (winh is null)
        tbkReport.Text = $"Window '{WinTitle}' not found";
      else
      {
        var text = await _ts.TabAndGetText(winh ?? throw new ArgumentNullException(nameof(s)));
        if (text.Length > 0)
        {
          tbkAnswer.Text =
          tbxPrompt.Text = text;
          return;
        }

        WriteLine(tbkReport.Text = $"FAILED TabAndGetText().");
      }
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally { bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; }
  }
  async void RunOnce(object s, RoutedEventArgs e) => await OnTimer("clk");
  async void ExitApp(object s, RoutedEventArgs e) { await bpr.FinishAsync(); Close(); }


  DateTime started;
  public void UpdateClock() { tbkTM.Text = (DateTime.Now - started).ToString("s\\.f"); }
  BackgroundTask? bt;
  void OnClockStart(object sender, RoutedEventArgs e)
  {
    bpr.Start();
    (bt ??= new(TimeSpan.FromSeconds(.1))).Start(UpdateClock);
    started = DateTime.Now;
  }

  async void OnClockStop(object sender, RoutedEventArgs e)
  {
    bpr.Finish();
    if (bt is not null) await bt.StopAsync();
  }
}// Tell me more about Ukraine.