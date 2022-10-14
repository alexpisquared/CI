namespace OpenAI;
public partial class MainWindow : Window
{
  const string _sarcazm = "Marv is a chatbot that reluctantly answers questions with sarcastic responses: ";
  readonly IConfigurationRoot _config;
  readonly Bpr bpr;
  readonly TextSender _ts = new();
  bool isTimer_On = false;
  DateTime started;
  BackgroundTask? btStopwatch, btTeamsChecker;

  public MainWindow()
  {
    InitializeComponent();
    DataContext = this;
    _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build(); //var secretProvider = _config.Providers.First(); if (secretProvider.TryGet("WhereAmI", out var secretPass))  WriteLine(secretPass);else  WriteLine("Hello, World!");
    bpr = new Bpr();
  }
  async void Window_Loaded(object s, RoutedEventArgs e)
  {
    //await bpr.StartAsync();
    if (DateTime.Now == DateTime.Today)
    {
      A:
      await bpr.GradientAsync(300, 600, 1); // 1.3 s
      await bpr.GradientAsync(600, 100, 1); // 1.3 s

      await bpr.GradientAsync(200, 400, 1); // 
      await bpr.GradientAsync(200, 400, 2); // 
      await bpr.GradientAsync(200, 600, 2); // 
      await bpr.GradientAsync(100, 800, 4); // 

      await bpr.GradientAsync(400, 100, 4); // 1.3 s
      await bpr.GradientAsync(400, 100, 2); // 1.3 s
      await bpr.GradientAsync(400, 100, 1); // 1.3 s


      await bpr.GradientAsync(200, 1100, 1); // 1.7 s
      await bpr.GradientAsync(100, 1100, 1); // 2.4 s - too vague at the start.
      await bpr.GradientAsync(500, 1100, 1); // 800 ms
      await bpr.GradientAsync(1100, 100, 1); // 2.4 s

      await bpr.GradientAsync(500, 1100, 2); // 400 ms
      await bpr.GradientAsync(1100, 500, 2); // 400 ms
      await bpr.GradientAsync(500, 1100, 4); // 400 ms
      await bpr.GradientAsync(1100, 500, 4); // 400 ms

      await bpr.GradientAsync(300, 1100, 2); // 600 ms
      await bpr.GradientAsync(1100, 300, 2); // 600 ms
      await bpr.GradientAsync(300, 1100, 4); // 300 ms  Start
      await bpr.GradientAsync(1100, 300, 4); // 300 ms  Finish

      await bpr.GradientAsync(100, 1100, 6); // 400 ms
      await bpr.GradientAsync(1100, 100, 6); // 400 ms



      await bpr.GradientAsync(100, 1100, 3); // 800 ms
      await bpr.GradientAsync(1100, 100, 3); // 800 ms

      //await bpr.GradientAsync(100, 2000, 2); // 1.5 s
      await bpr.GradientAsync(2000, 100, 2); // 1.5 s
      await bpr.GradientAsync(100, 2000, 5); // 600 ma
      await bpr.GradientAsync(2000, 100, 5); // 600 ms

      //await bpr.WaveAsync(1000, 1100, 1);

      //await bpr.WaveAsync(3000, 3300, 4);
      //await bpr.WaveAsync(3000, 3300, 3);
      //await bpr.WaveAsync(3000, 3300, 2);
      //await bpr.GradientAsync(300, 1300, 1);
      await bpr.GradientAsync(1300, 300, 1);
      //await bpr.GradientAsync(500, 1500, 1);

      await bpr.GradientAsync(900, 1500, 1); // 500 ms
      await bpr.GradientAsync(1500, 900, 1); // 500 ms
      await bpr.GradientAsync(900, 1500, 2); // 250 ms
      await bpr.GradientAsync(1500, 900, 2); // 250 ms
      await bpr.GradientAsync(800, 1600, 2); // 350 ms
      await bpr.GradientAsync(1600, 800, 2); // 350 ms
      await bpr.GradientAsync(800, 1600, 3); // 232 ms
      await bpr.GradientAsync(1600, 800, 3); // 232 ms

      //await bpr.GradientAsync(100, 1100, 1);
      await bpr.GradientAsync(1200, 2000, 1);
      await bpr.GradientAsync(1400, 2000, 1);
      await bpr.GradientAsync(1600, 2000, 1);
      await bpr.GradientAsync(1800, 2000, 1);
      await bpr.GradientAsync(2000, 3000, 1);
      await bpr.GradientAsync(3000, 4000, 1);
      await bpr.GradientAsync(4000, 5000, 1);
      await bpr.GradientAsync(5000, 6000, 1);
      //await bpr.GradientAsync(6000, 7000, 1);
      goto A;
    }

    bpr.AppStart();

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
  async void UpdateStopwatch() { tbkTM.Text = (DateTime.Now - started).ToString("s\\.f"); await Task.Delay(2); }
  async void OnTimerVoid() => await OnTimerTask();
  async Task OnTimerTask()
  {
    if (!EnabledY) return;

    EnabledY = IsWaiting = false;
    await AllDialogSteps();
    EnabledY = IsWaiting = true;
  }
  async Task AllDialogSteps()
  {
    WriteLine($"\n>>> Starting ... ");

    var questn = await ScrapeTAsync();
    if (tbxPrompt.Text == questn)        /**/ { tbkReport.Foreground = Brushes.Magenta; WriteLine(tbkReport.Text = $"Ignoring the same msg: '{TextSender.SafeLengthTrim(questn)}'."); return; }
    if (tbkAnswer.Text.Contains(questn)) /**/ { tbkReport.Foreground = Brushes.Magenta; WriteLine(tbkReport.Text = $"Ignoring prev answer: '{TextSender.SafeLengthTrim(questn)}'."); return; }

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
  public bool IsTimer_On { get => isTimer_On; set => IsWaiting = isTimer_On = value; }
  public bool IsAutoQrAI { get; set; } = true;
  public bool IsAutoAnsr { get; set; } = false;
  public bool IsSarcastc { get; set; } = true;
  public bool IsWebTeams { get; set; } = true;
  public static readonly DependencyProperty WinTitleProperty = DependencyProperty.Register("WinTitle", typeof(string), typeof(MainWindow)); public string WinTitle { get => (string)GetValue(WinTitleProperty); set => SetValue(WinTitleProperty, value); }
  public static readonly DependencyProperty EnabledYProperty = DependencyProperty.Register("EnabledY", typeof(bool), typeof(MainWindow)); public bool EnabledY { get => (bool)GetValue(EnabledYProperty); set => SetValue(EnabledYProperty, value); }
  public static readonly DependencyProperty IsWaitingProperty = DependencyProperty.Register("IsWaiting", typeof(bool), typeof(MainWindow)); public bool IsWaiting { get => (bool)GetValue(IsWaitingProperty); set => SetValue(IsWaitingProperty, value); }

  async Task<string> ScrapeTAsync()
  {
    var sw = Stopwatch.StartNew(); tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = "Scraping Teams' last msg..."); //menu1.IsEnabled = false;

    _ = Mouse.Capture(this);
    var current = PointToScreen(Mouse.GetPosition(this));
    try
    {
      IntPtr? winh;
      try
      {
        Hide();
        winh = await _ts.GetFirstMatch(IsWebTeams ? "msedge" : _config["Prc"], WinTitle, byEndsWith: IsWebTeams ? null : true);

        var (right, bottom) = _ts.GetRB(winh ?? throw new ArgumentNullException(nameof(winh)));
        await MouseOperations.MouseClickEventAsync(right - 32, bottom - 400);
        if (!IsWebTeams)
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
    }
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
      var winh = await _ts.GetFirstMatch(IsWebTeams ? "msedge" : _config["Prc"], WinTitle, byEndsWith: IsWebTeams ? null : true);
      if (winh is null)
      {
        tbkReport.Foreground = Brushes.Pink;
        tbkReport.Text = $"Window '{WinTitle}' not found";
      }
      else
      {
        Hide();
        var (right, bottom) = _ts.GetRB(winh ?? throw new ArgumentNullException(nameof(winh)));
        await MouseOperations.MouseClickEventAsync(right - 520, bottom - 88 + (IsWebTeams ? 0 : 0));
        if (!IsWebTeams)
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
          await bpr.WaveAsync3k8k4();// BeepAsync(6000, .333);
          await Task.Delay(125);
        }

        await bpr.GradientAsync(800, 300, 2); // ~800 ms                   await bpr.WaveAsync7k5k1();
      }
    }
    catch (ArgumentOutOfRangeException ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); }
    catch (Exception ex) { tbkReport.Foreground = Brushes.Orange; WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
    finally
    {
      //if (IsWebTeams)        MouseOperations.SetCursorPosition((int)ptsPosn.X, (int)ptsPosn.Y);

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

      (btStopwatch ??= new(TimeSpan.FromSeconds(.1))).Start(UpdateStopwatch);
      started = DateTime.Now;

      var query = $"{(IsSarcastc ? _sarcazm : "")}" + tbxPrompt.Text
        //.Replace("'", "\\'")
        //.Replace("\"", "\\\"")
        ;

      var (ts, finishReason, answer) = await OpenAILib.OpenAI.CallOpenAI(_config, tbkMax.Text, query);

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
      if (btStopwatch is not null) await btStopwatch.StopAsync(); btStopwatch = null;
    }
  }
  async void QueryAI(object s, RoutedEventArgs e) => await QueryAiAsync(s);
  async void SetText(object s, RoutedEventArgs e) { bpr.Start(); Clipboard.SetText(tbkAnswer.Text); await Task.Yield(); }
  async void ScrapeT(object s, RoutedEventArgs e) => tbxPrompt.Text = await ScrapeTAsync();
  async void TypeMsg(object s, RoutedEventArgs e) => await TypeMsgAsync();
  async void TabNSee(object s, RoutedEventArgs e)
  {
    var sw = Stopwatch.StartNew(); bpr.Start(); tbkReport.Foreground = Brushes.Gray; WriteLine(tbkReport.Text = "TabNSee()...");

    try
    {
      var winh = await _ts.GetFirstMatch(IsWebTeams ? "msedge" : _config["Prc"], WinTitle, byEndsWith: IsWebTeams ? null : true);
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
  async void ExitApp(object s, RoutedEventArgs e) { Hide(); await bpr.AppFinishAsync(); Close(); }

  async void OnTeamsCheckerStart(object s, RoutedEventArgs e) { await bpr.StartAsync(); (btTeamsChecker ??= new((Resources["WaitDuration"] as Duration?)?.TimeSpan ?? TimeSpan.FromSeconds(20))).Start(OnTimerVoid); }
  async void OnTeamsCheckerStop(object s, RoutedEventArgs e) { bpr.Finish(); if (btTeamsChecker is not null) await btTeamsChecker.StopAsync(); btTeamsChecker = null; }
  async void OnStopwatchStart(object s, RoutedEventArgs e) { await bpr.StartAsync(); (btStopwatch ??= new(TimeSpan.FromSeconds(.1))).Start(UpdateStopwatch); started = DateTime.Now; }
  async void OnStopwatchStop(object s, RoutedEventArgs e) { bpr.Finish(); if (btStopwatch is not null) await btStopwatch.StopAsync(); btStopwatch = null; }
}