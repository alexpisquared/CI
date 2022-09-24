namespace OpenAI;
public partial class MainWindow : Window
{
  readonly TimeSpan _waitDuration;
  readonly IConfigurationRoot _config;
  readonly Bpr bpr;
  readonly TextSender _ts = new();
  string _prevClpbrd = "";
  bool isTimer_On = false;
  DateTime started;
  BackgroundTask? btClock, btAat;

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

    EnabledY = true;
    await Task.Yield();
  }
  async void UpdateClock() { tbkTM.Text = (DateTime.Now - started).ToString("s\\.f"); await Task.Delay(2); }
  async void OnTimerVoid() => await OnTimerTask();
  async Task OnTimerTask()
  {
    var thread = "timer";
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
    if (tbxPrompt.Text == questn)        /**/ { tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = $"Ignoring the same msg: '{TextSender.SafeLengthTrim(questn)}'."); return; }

    if (tbkAnswer.Text.Contains(questn)) /**/ { tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = $"Ignoring prev answer: '{TextSender.SafeLengthTrim(questn)}'."); return; }

    tbxPrompt.Text = questn;

    if (IsAutoQrAI)
      await QueryAiAsync(btnQryAI);
    else
      tbkAnswer.Text = tbxPrompt.Text;

    if (IsAutoAnsr)
      await TypeMsgAsync();
    else
      tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = "Auto type is OFF.");
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
      if (!Clipboard.ContainsText() || _prevClpbrd == Clipboard.GetText()) { tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = $"Same   [{DateTime.Now:ss}]"); bpr.Error(); return; }

      _prevClpbrd = Clipboard.GetText();
      if (_prevClpbrd.Length < minLen) { tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = $"Too Small"); bpr.Start(); bpr.Error(); return; }

      tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = $"Valid");
      tbxPrompt.Text = _prevClpbrd.Trim();

      if (IsAutoQrAI) await QueryAiAsync(btnQryAI);
      if (IsAutoAnsr) TypeMsg(btnQryAI, new RoutedEventArgs());

      bpr.Error();
    }
    catch (Exception ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
  }
  public bool IsTimer_On { get => isTimer_On; set => IsWaiting = isTimer_On = value; }
  public bool IsAutoQrAI { get; set; } = false;
  public bool IsAutoAnsr { get; set; } = false;
  public static readonly DependencyProperty WinTitleProperty = DependencyProperty.Register("WinTitle", typeof(string), typeof(MainWindow)); public string WinTitle { get => (string)GetValue(WinTitleProperty); set => SetValue(WinTitleProperty, value); }
  public static readonly DependencyProperty EnabledYProperty = DependencyProperty.Register("EnabledY", typeof(bool), typeof(MainWindow)); public bool EnabledY { get => (bool)GetValue(EnabledYProperty); set => SetValue(EnabledYProperty, value); }
  public static readonly DependencyProperty IsWaitingProperty = DependencyProperty.Register("IsWaiting", typeof(bool), typeof(MainWindow)); public bool IsWaiting { get => (bool)GetValue(IsWaitingProperty); set => SetValue(IsWaitingProperty, value); }

  async Task<string> ScrapeTAsync()
  {
    var sw = Stopwatch.StartNew(); tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = "Scraping Teams' last msg..."); //menu1.IsEnabled = false;

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
    catch (ArgumentOutOfRangeException ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (IndexOutOfRangeException ex)/**/{ tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (Exception ex)               /**/{ tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }

    try
    {
      var text = await ScrapeLastMsgFromteams(winh ?? throw new ArgumentNullException(nameof(winh), $"Window '{WinTitle}' not found"));
      return text;
    }
    catch (ArgumentOutOfRangeException ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (IndexOutOfRangeException ex)/**/{ tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    catch (Exception ex)               /**/{ tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); return ex.Message; }
    finally
    {
      Show();
      tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; //menu1.IsEnabled = true;

      _ = Mouse.Capture(null);
    }
  }
  async Task TypeMsgAsync()
  {
    var sw = Stopwatch.StartNew(); tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = "Typing into Teams..."); tbkAnswer.Background = Brushes.DarkRed;

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

        foreach (var paragrapf in tbkAnswer.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
          var failReport = _ts.SendMsg(winh ?? throw new ArgumentNullException(nameof(winh)), $"{paragrapf}{{ENTER}}");
          if (failReport.Length > 0)
          {
            tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = $"FAILED SendKey(): {failReport}");
            return;
          }
          await bpr.BeepAsync(7000, .333);
          await Task.Delay(100);
        }

        await bpr.WaveAsync3k8k();
      }
    }
    catch (ArgumentOutOfRangeException ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally
    {
      _ = Mouse.Capture(null);
      Show();
      tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; tbkAnswer.Background = Brushes.Transparent;
    }
  }
  async Task QueryAiAsync(object s)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = "Asking AI ..."); tbkAnswer.Background = Brushes.DarkOliveGreen; ((Control)s).Visibility = Visibility.Hidden;
    try
    {
      tbkAnswer.Text = "Asking AI ...";

      (btClock ??= new(TimeSpan.FromSeconds(.1))).Start(UpdateClock);
      started = DateTime.Now;

      var (ts, finishReason, answer) = await OpenAILib.OpenAI.CallOpenAI(_config, tbkMax.Text, tbxPrompt.Text);

      tbkAnswer.Text = answer.StartsWith("\n") ? answer.Trim('\n') : $"{tbxPrompt.Text}{answer}";
      tbkTM.Text = $"{ts.TotalSeconds:N1}";
      tbkFR.Text = finishReason;
      tbkLn.Text = $"{answer.Length}";
      tbkZZ.Text = "·";
    }
    catch (Exception ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally
    {
      bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; tbkAnswer.Background = Brushes.Transparent; ((Control)s).Visibility = Visibility.Visible;
      if (btClock is not null) await btClock.StopAsync(); btClock = null;
    }
  }
  async void QueryAI(object s, RoutedEventArgs e) => await QueryAiAsync(s);
  async void SetText(object s, RoutedEventArgs e) { bpr.Start(); _prevClpbrd = tbkAnswer.Text; Clipboard.SetText(tbkAnswer.Text); await Task.Yield(); }
  async void ScrapeT(object s, RoutedEventArgs e) => tbxPrompt.Text = await ScrapeTAsync();
  async void TypeMsg(object s, RoutedEventArgs e) => await TypeMsgAsync();
  async void TabNSee(object s, RoutedEventArgs e)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = "TabNSee()...");

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

        tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = $"FAILED TabAndGetText().");
      }
    }
    catch (ArgumentOutOfRangeException ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally { bpr.Finish(); tbkReport.Text += $"  {sw.Elapsed.TotalSeconds:N1}s"; }
  }
  async void RunOnce(object s, RoutedEventArgs e) => await OnTimerTask();
  async void ExitApp(object s, RoutedEventArgs e) { await bpr.FinishAsync(); Close(); }

  async void OnClockStart(object s, RoutedEventArgs e) { await bpr.StartAsync(); (btClock ??= new(TimeSpan.FromSeconds(.1))).Start(UpdateClock); started = DateTime.Now; }
  async void OnClockStop(object s, RoutedEventArgs e) { bpr.Finish(); if (btClock is not null) await btClock.StopAsync(); btClock = null; }
  async void OnAutoAnswerTimerStart(object s, RoutedEventArgs e) { await bpr.StartAsync(); (btAat ??= new(TimeSpan.FromSeconds(10))).Start(OnTimerVoid); }
  async void OnAutoAnswerTimerStop(object s, RoutedEventArgs e) { bpr.Finish(); if (btAat is not null) await btAat.StopAsync(); btAat = null; }
}// Tell me more about Ukraine.