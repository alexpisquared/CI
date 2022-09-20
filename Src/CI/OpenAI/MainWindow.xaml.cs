namespace OpenAI;
public partial class MainWindow : Window
{
  const int _checkPeriodSec = 15;
  readonly IConfigurationRoot _config;
  readonly Bpr bpr;
  readonly TextSender _ts = new();
  CancellationTokenSource? _cts;
  string _prevClpbrd = "", _prevReader = "", _prevMsgTpd = "";
  bool _alreadyChecking;

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
      "Microsoft Teams"};
    cbxTeams.SelectedIndex = 0;

    _ = tbxPrompt.Focus();

    //_ts.FindByProc(_config["Prc"], WinTitle);

    await BlockingTimer();
  }
  void DeblockingTimer() => Task.Run(async () => await BlockingTimer());
  async Task BlockingTimer()
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
            await CheckEndpoints("UI");
          else
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(async () => { await CheckEndpoints("bg"); }));
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
  async Task CheckEndpoints(string thread)
  {
    if (_alreadyChecking) return;

    WriteLine($"\n>>> Starting on  '{thread}' ... ");

    _alreadyChecking = true;
    bpr.Start();
    await OnCheckTeamsForCahnges();
    _alreadyChecking = false;
  }
  async Task OnCheckTeamsForCahnges()
  {
    try
    {
      var (reader, writer, whyFailed) = await _ts.GetTwoWinndows(_config["Prc"], WinTitle, bpr.Tick);

      if (!string.IsNullOrEmpty(whyFailed)) { WriteLine(tbkReport.Text = whyFailed); bpr.Error(); return; }
      if (writer is null) { WriteLine(tbkReport.Text = "Error: Unable to read Sender"); bpr.Error(); return; }
      if (reader is null) { WriteLine(tbkReport.Text = "Error: Unable to read All   "); bpr.Error(); return; }

      var ary = await ScrapeLastMsgFromteams(reader ?? throw new ArgumentNullException());

      tbxPrompt.Text = ary;

      if (IsAutoQrAI)
        await QueryAiAsync(btnQryAI, new RoutedEventArgs());
      else
        tbkAnswer.Text = tbxPrompt.Text;

      if (!IsAutoType)
        WriteLine(tbkReport.Text = "Auto type is OFF.");
      else
      {
        //if (_prevMsgTpd == tbxPrompt.Text)           WriteLine(tbkReport.Text = "Same msg already typed in.");        else
        {
          _prevMsgTpd = tbxPrompt.Text;

          WriteLine(tbkReport.Text = $"Typing   '{tbkAnswer.Text}' ...");

          var failMsg = _ts.SendMsg(writer ?? default, IsAutoEntr ? $"{tbkAnswer.Text}{{ENTER}}" : tbkAnswer.Text);
          if (!string.IsNullOrEmpty(failMsg))
            WriteLine(tbkReport.Text = failMsg);
          else
          {
            WriteLine(tbkReport.Text = string.IsNullOrEmpty(failMsg) ? $"Success answering to  '{tbxPrompt.Text}'  with  {tbkAnswer.Text.Length} char long ancwer; " : failMsg);

            await AddSpacerToWriterWindow(writer);
          }
        }
      }

      bpr.Error();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
  }

  async Task<string> ScrapeLastMsgFromteams(IntPtr winh)
  {
    var read = await _ts.GetTargetTextFromWindow(winh);

    //if (_prevReader == read) { WriteLine(tbkReport.Text = $"Same (len {read.Length}) read.    [{DateTime.Now:ss}]  "); bpr.Error(); throw new ArgumentOutOfRangeException($"AP: {tbkReport.Text}"); }

    _prevReader = read;
    //if (read.Length < minLen) { WriteLine(tbkReport.Text = $"Too Small: '{read}'.    [{DateTime.Now:ss}]           "); bpr.Error(); throw new ArgumentOutOfRangeException($"AP: {tbkReport.Text}"); }

    var ary = read.Trim().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

    //TMI: hundreds of lines!!! for (var i = 0; i < ary.Length; i++) { WriteLine($".. {i,5})   {ary[i]}"); } 

    for (var i = 5 - 1; i >= 1; i--) { WriteLine($"·· {ary.Length - i,5})   {ary[^i]}"); }

    return ary[^1];
  }

  async Task AddSpacerToWriterWindow(IntPtr? writer)
  {
    for (var i = 0; i < 100; i++)
    {
      var spacer = "     ";
      var failMsg = _ts.SendMsg(writer ?? default, spacer);
      if (!string.IsNullOrEmpty(failMsg))
        WriteLine(tbkReport.Text = failMsg);

      var rv = await _ts.GetTwoWinndows(_config["Prc"], WinTitle, bpr.Tick);
      if (await _ts.GetTargetTextFromWindow(rv.writer ?? default) == spacer)
      {
        WriteLine(tbkReport.Text += $" spaced on {i}.");
        break;
      }

      await Task.Delay(100);
      Write($"{i} ");
    }
  }

  async void OnCheckClipboardForData(string thread)
  {
    const int minLen = 10;
    try
    {
      if (!Clipboard.ContainsText() || _prevClpbrd == Clipboard.GetText()) { WriteLine(tbkReport.Text = $"Same   [{DateTime.Now:ss}]"); bpr.Error(); return; }

      _prevClpbrd = Clipboard.GetText();
      if (_prevClpbrd.Length < minLen) { WriteLine(tbkReport.Text = $"Too Small"); bpr.Start(); bpr.Error(); return; }

      WriteLine(tbkReport.Text = $"Valid");
      tbxPrompt.Text = _prevClpbrd.Trim();

      if (IsAutoQrAI) await QueryAiAsync(btnQryAI, new RoutedEventArgs());
      if (IsAutoType) TypeMsg(btnQryAI, new RoutedEventArgs());

      bpr.Error();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
  }

  public bool IsTimer_On { get; set; }
  public bool IsAutoQrAI { get; set; } = true;
  public bool IsAutoType { get; set; } = true;
  public bool IsAutoEntr { get; set; }
  public static readonly DependencyProperty WinTitleProperty = DependencyProperty.Register("WinTitle", typeof(string), typeof(MainWindow)); public string WinTitle { get { return (string)GetValue(WinTitleProperty); } set { SetValue(WinTitleProperty, value); } }

  async void QueryAI(object s, RoutedEventArgs e) => await QueryAiAsync(s, e);

  async Task QueryAiAsync(object s, RoutedEventArgs e)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Asking AI ...");
    try
    {
      ((Control)s).Visibility = Visibility.Hidden;

      tbkAnswer.Text = "Asking AI ...";

      var (ts, finishReason, answer) = await OpenAILib.OpenAI.CallOpenAI(_config, 1250, tbxPrompt.Text);

      tbkAnswer.Text = answer;
      tbkTM.Text = $"{ts.TotalSeconds:N1}";
      tbkFR.Text = finishReason;
      tbkLn.Text = $"{answer.Length}";
      tbkZZ.Text = "·";

      //_ = tbxPrompt.Focus();
      //SetText(s, e);
    }
    finally { bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; ((Control)s).Visibility = Visibility.Visible; }
  }

  void SetText(object s, RoutedEventArgs e) { bpr.Start(); _prevClpbrd = tbkAnswer.Text; Clipboard.SetText(tbkAnswer.Text); }

  async void ScrapeT(object sender, RoutedEventArgs e)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Scraping Teams' last msg...");

    try
    {
      var winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);
      if (winh is null)
        tbkReport.Text = $"Window '{WinTitle}' not found";
      else
        tbxPrompt.Text = await ScrapeLastMsgFromteams(winh ?? throw new ArgumentNullException());
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); }
    catch (IndexOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally { bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; }
  }
  async void TypeMsg(object s, RoutedEventArgs e)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "Typing into Teams...");

    try
    {
      var winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);
      if (winh is null)
        tbkReport.Text = $"Window '{WinTitle}' not found";
      else
      {
        var failReport = _ts.SendMsg(winh ?? throw new ArgumentNullException(), IsAutoEntr ? $"{tbkAnswer.Text}{{ENTER}}" : tbkAnswer.Text);
        if (failReport.Length > 0)
        {
          WriteLine(tbkReport.Text = $"FAILED SendKey(): {failReport}");
          return;
        }

        if (IsAutoEntr)
        {
          await AddSpacerToWriterWindow(winh);
        }
      }
    }
    catch (ArgumentOutOfRangeException ex) { WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally { bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; }
  }

  async void TabNSee(object sender, RoutedEventArgs e)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); WriteLine(tbkReport.Text = "TabNSee()...");

    try
    {
      var winh = await _ts.GetFirstMatch(_config["Prc"], WinTitle, byEndsWith: true);
      if (winh is null)
        tbkReport.Text = $"Window '{WinTitle}' not found";
      else
      {
        var text = await _ts.TabAndGetText(winh ?? throw new ArgumentNullException());
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

  async void RunOnce(object s, RoutedEventArgs e) => await CheckEndpoints("clk");
  async void ExitApp(object s, RoutedEventArgs e) { await bpr.FinishAsync(); Close(); }
}
// Tell me more about Ukraine.
