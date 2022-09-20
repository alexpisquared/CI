using System.Diagnostics;

namespace OpenAI;
public partial class MainWindow : Window
{
  const int _checkPeriodSec = 15;
  readonly IConfigurationRoot _config;
  readonly TextSender _ts = new();
  CancellationTokenSource? _cts;
  string _prevClpbrd = "", _prevReader = "", _prevMsgTpd = "";
  private bool _alreadyChecking;
  private bool isTimer_On;

  public MainWindow()
  {
    InitializeComponent();
    DataContext = this;
    _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build(); //var secretProvider = _config.Providers.First(); if (secretProvider.TryGet("WhereAmI", out var secretPass))  WriteLine(secretPass);else  WriteLine("Hello, World!");
  }

  async void Window_Loaded(object s, RoutedEventArgs e)
  {
    EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); }));
    MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
    _ = tbxPrompt.Focus();

    _ts.FindByProc(_config["Prc"], _config["Ttl"]);

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
            CheckEndpoints("UI");
          else
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { CheckEndpoints("bg"); }));
        }
        else
          WriteLine(tbkReport.Text = "Off");

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
  void CheckEndpoints(string thread)
  {
    if (_alreadyChecking) return;

    WriteLine($"\n>>> Starting on  '{thread}' ... ");

    _alreadyChecking = true;
    SystemSounds.Beep.Play();
    OnCheckTeamsForCahnges();
    _alreadyChecking = false;
  }
  async void OnCheckTeamsForCahnges()
  {
    const int minLen = 10;
    try
    {
      var (reader, writer, whyFailed) = _ts.GetTwoWinndows(_config["Prc"], _config["Ttl"]);

      if (!string.IsNullOrEmpty(whyFailed)) { WriteLine(tbkReport.Text = whyFailed); SystemSounds.Hand.Play(); return; }

      if (reader is null) { WriteLine(tbkReport.Text = "Error: Unable to read All   "); SystemSounds.Hand.Play(); return; }

      if (writer is null) { WriteLine(tbkReport.Text = "Error: Unable to read Sender"); SystemSounds.Hand.Play(); return; }

      var read = _ts.GetTargetTextFromWindow(reader ?? default);

      if (_prevReader == read) { WriteLine(tbkReport.Text = $"Same (len {read.Length}) read.    [{DateTime.Now:ss}]  "); SystemSounds.Hand.Play(); return; }

      _prevReader = read;
      if (read.Length < minLen) { WriteLine(tbkReport.Text = $"Too Small: '{read}'.    [{DateTime.Now:ss}]  "); SystemSounds.Hand.Play(); return; }

      WriteLine(tbkReport.Text = $"Valid question: '{read}'.");

      var ary = read.Trim().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

      for (var i = 0; i < ary.Length; i++) { WriteLine($"{i,5})   {ary[i]}"); } // foreach (var ary2 in ary)WriteLine($"{i,3}) {ary2}");

      tbxPrompt.Text = ary[^1];

      if (IsAutoQrAI)
      {
        IsTimer_On = false;
        await QueryAiAsync(btnQryAI, new RoutedEventArgs());
        IsTimer_On = true;
      }

      if (IsAutoType)
      {
        if (_prevMsgTpd != tbxPrompt.Text) //TypeMsg(1, new RoutedEventArgs());
        {
          WriteLine(tbkReport.Text = $"Typing   '{tbxPrompt.Text}' ...");

          var failMsg = _ts.SendMsg(writer ?? default, $"{(IsAutoQrAI ? tbkAnswer.Text : tbxPrompt.Text)}  {{ENTER}}");
          if (string.IsNullOrEmpty(failMsg))
          {
            _prevMsgTpd = tbxPrompt.Text;
            WriteLine(tbkReport.Text = string.IsNullOrEmpty(failMsg) ? $"Success typing in  '{tbxPrompt.Text}'." : failMsg);
            Thread.Sleep(100);
            var failMs2 = _ts.SendMsg(writer ?? default, $"  ");
            WriteLine(tbkReport.Text = string.IsNullOrEmpty(failMs2) ? "Success adding '  '." : failMs2);
          }
          else
            WriteLine(tbkReport.Text = failMsg);

        }
        else
          WriteLine(tbkReport.Text = "Same msg already typed in.");
      }
      else
        WriteLine(tbkReport.Text = "Auto type is OFF.");

      SystemSounds.Hand.Play();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
  }
  async void OnCheckClipboardForData(string thread)
  {
    const int minLen = 10;
    try
    {
      if (!Clipboard.ContainsText() || _prevClpbrd == Clipboard.GetText()) { WriteLine(tbkReport.Text = $"Same   [{DateTime.Now:ss}]"); SystemSounds.Hand.Play(); return; }

      _prevClpbrd = Clipboard.GetText();
      if (_prevClpbrd.Length < minLen) { WriteLine(tbkReport.Text = $"Too Small"); SystemSounds.Beep.Play(); SystemSounds.Hand.Play(); return; }

      WriteLine(tbkReport.Text = $"Valid");
      tbxPrompt.Text = _prevClpbrd.Trim();

      if (IsAutoQrAI) await QueryAiAsync(btnQryAI, new RoutedEventArgs());
      if (IsAutoType) TypeMsg(btnQryAI, new RoutedEventArgs());

      SystemSounds.Hand.Play();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); if (Debugger.IsAttached) Debugger.Break(); }
  }

  public bool IsTimer_On
  {
    get => isTimer_On; set
    {
      isTimer_On = value;
      CheckEndpoints("set");
    }
  }
  public bool IsAutoQrAI { get; set; }
  public bool IsAutoType { get; set; } = true;
  async void QueryAI(object s, RoutedEventArgs e) => await QueryAiAsync(s, e);

  async Task QueryAiAsync(object s, RoutedEventArgs e)
  {
    try
    {
      ((Control)s).IsEnabled = false;

      tbkAnswer.Text = "Sending ...";

      var (ts, finishReason, answer) = await OpenAILib.OpenAI.CallOpenAI(_config, 1250, tbxPrompt.Text);

      tbkAnswer.Text = answer;
      tbkTM.Text = $"{ts.TotalSeconds:N1}";
      tbkFR.Text = finishReason;
      tbkLn.Text = $"{answer.Length}";
      tbkZZ.Text = "·";

      _ = tbxPrompt.Focus();
      SetText(s, e);
    }
    finally
    {
      ((Control)s).IsEnabled = true;
    }
  }

  void SetText(object s, RoutedEventArgs e) { SystemSounds.Beep.Play(); _prevClpbrd = tbkAnswer.Text; Clipboard.SetText(tbkAnswer.Text); }

  void RunOnce(object s, RoutedEventArgs e)
  {
    CheckEndpoints("clk");
  }

  void TypeMsg(object s, RoutedEventArgs e)
  {
    SystemSounds.Beep.Play();
    var rv = _ts.SendOnce(tbkAnswer.Text, _config["Ttl"]);
    WriteLine(tbkReport.Text = $"> SendKey result: {(rv.Length == 0 ? "Suceess!" : rv)}");
  }
  void ExitApp(object s, RoutedEventArgs e) { SystemSounds.Beep.Play(); Close(); }
}
// Tell me more about Ukraine.
