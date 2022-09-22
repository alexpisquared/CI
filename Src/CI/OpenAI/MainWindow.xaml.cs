namespace OpenAI;
public partial class MainWindow : Window
{
  const int _checkPeriodSec = 15;
  readonly IConfigurationRoot _config;
  readonly Bpr bpr;
  readonly TextSender _ts = new();
  CancellationTokenSource? _cts;
  string _prevClpbrd = "";
  public MainWindow()
  {
    InitializeComponent();
    DataContext = this;
    _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build(); //var secretProvider = _config.Providers.First(); if (secretProvider.TryGet("WhereAmI", out var secretPass))  WriteLine(secretPass);else  WriteLine("Hello, World!");
    bpr = new Bpr();
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
      using PeriodicTimer tmr = new(TimeSpan.FromSeconds(_checkPeriodSec));
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
        //else          WriteLine(tbkReport.Text = "Off");

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

    await AllDialogSteps();
    EnabledY = true;
  }
  async Task AllDialogSteps()
  {
    var questn = await ScrapeTAsync();
    if (tbxPrompt.Text == questn)        /**/ { WriteLine(tbkReport.Text = $"Same msg: ignored!"); return; }
    if (tbkAnswer.Text.Contains(questn)) /**/ { WriteLine(tbkReport.Text = $"This is my last answer: ignored!"); return; }

    tbxPrompt.Text = questn;

    if (IsAutoQrAI)
      await QueryAiAsync(btnQryAI);
    else
      tbkAnswer.Text = tbxPrompt.Text;

    if (IsAutoType)
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

    return ary[^1];
  }
  async Task AddSpacerToWriterWindow(IntPtr? winh)
  {
    for (var i = 0; i < 100; i++)
    {
      var spacer = "     ";
      var failMsg = _ts.SendMsg(winh ?? default, spacer);
      if (!string.IsNullOrEmpty(failMsg))
        WriteLine(tbkReport.Text = failMsg);

      if (await _ts.GetTargetTextFromWindow(winh ?? default, bpr.Tick) == spacer)
      {
        WriteLine(tbkReport.Text += $" spaced on {i}.");
        break;
      }

      await Task.Delay(100);
      Write($"{i} ");
    }
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
      if (IsAutoType) TypeMsg(btnQryAI, new RoutedEventArgs());

      bpr.Error();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
  }
  public bool IsTimer_On { get; set; }
  public bool IsAutoQrAI { get; set; } = true;
  public bool IsAutoType { get; set; } = true;
  public bool IsAutoEntr { get; set; }
  public static readonly DependencyProperty WinTitleProperty = DependencyProperty.Register("WinTitle", typeof(string), typeof(MainWindow)); public string WinTitle { get => (string)GetValue(WinTitleProperty); set => SetValue(WinTitleProperty, value); }
  public static readonly DependencyProperty EnabledYProperty = DependencyProperty.Register("EnabledY", typeof(bool), typeof(MainWindow)); public bool EnabledY { get => (bool)GetValue(EnabledYProperty); set => SetValue(EnabledYProperty, value); }
  async Task<string> ScrapeTAsync()
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Scraping Teams' last msg..."); //menu1.IsEnabled = false;

    _ = Mouse.Capture(this);
    var current = PointToScreen(Mouse.GetPosition(this));

    MouseOperations.MouseClickEvent(960, 900);   // await Task.Delay(8); // needs time to realise that ~at the new spot already

    IntPtr? winh;
    try
    {
      winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); return ex.Message; }
    catch (IndexOutOfRangeException ex)/**/{ WriteLine(tbkReport.Text = ex.Message); return ex.Message; }
    catch (Exception ex)               /**/{ WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }

    try
    {
      var text = await ScrapeLastMsgFromteams(winh ?? throw new ArgumentNullException(nameof(winh), $"Window '{WinTitle}' not found"));
      return text;
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); return ex.Message; }
    catch (IndexOutOfRangeException ex)/**/{ WriteLine(tbkReport.Text = ex.Message); return ex.Message; }
    catch (Exception ex)               /**/{ WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    finally
    {
      bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; //menu1.IsEnabled = true;

      MouseOperations.SetCursorPosition((int)current.X, (int)current.Y);
      _ = Mouse.Capture(null);
    }
  }
  async Task TypeMsgAsync()
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Typing into Teams...");

    _ = Mouse.Capture(this);
    var current = PointToScreen(Mouse.GetPosition(this));

    MouseOperations.MouseClickEvent(960, 960); await Task.Delay(40);

    try
    {
      var winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);
      if (winh is null)
        tbkReport.Text = $"Window '{WinTitle}' not found";
      else
      {
        var failReport = _ts.SendMsg(winh ?? throw new ArgumentNullException(nameof(winh)), IsAutoEntr ? $"{tbkAnswer.Text}{{ENTER}}" : tbkAnswer.Text);
        if (failReport.Length > 0)
        {
          WriteLine(tbkReport.Text = $"FAILED SendKey(): {failReport}");
          return;
        }

        //if (IsAutoEntr) { await AddSpacerToWriterWindow(winh); }
      }
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally
    {
      bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s";

      MouseOperations.SetCursorPosition((int)current.X, (int)current.Y);
      _ = Mouse.Capture(null);
    }
  }
  async Task QueryAiAsync(object s)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Asking AI ...");
    try
    {
      ((Control)s).Visibility = Visibility.Hidden;

      tbkAnswer.Text = "Asking AI ...";

      var (ts, finishReason, answer) = await OpenAILib.OpenAI.CallOpenAI(_config, 1250, tbxPrompt.Text);

      tbkAnswer.Text = answer.StartsWith("\n") ? answer.Trim('\n') : $"{tbxPrompt.Text}{answer}";
      tbkTM.Text = $"{ts.TotalSeconds:N1}";
      tbkFR.Text = finishReason;
      tbkLn.Text = $"{answer.Length}";
      tbkZZ.Text = "·";
    }
    finally { bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; ((Control)s).Visibility = Visibility.Visible; }
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
}// Tell me more about Ukraine.