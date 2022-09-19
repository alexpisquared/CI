using System.Reflection.Metadata;
using System.Windows.Interop;
namespace OpenAI;
public partial class MainWindow : Window
{
  const int _checkPeriodSec = 8;
  readonly IConfigurationRoot _config;
  readonly TextSender _ts = new TextSender();
  CancellationTokenSource? _cts;
  string _prevClpbrd = "", _prevReader = "", _prevMsgTpd = "";

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
    tbxPrompt.Focus();

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
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { CheckEndpoints("--"); }));
        }
        else
          tbkReport.Text = tbkStatus.Text = "Off";

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
  void CheckEndpoints(string thread) { SystemSounds.Beep.Play(); /*Write($" {thread} ")*/; OnCheckTeamsForCahnges(); }
  void OnCheckTeamsForCahnges()
  {
    const int minLen = 10;
    try
    {
      var (reader, writer, whyFailed) = _ts.GetTwoWinndows(_config["Prc"], _config["Ttl"]);

      if (!string.IsNullOrEmpty(whyFailed)) { tbkReport.Text = whyFailed; SystemSounds.Hand.Play(); return; }
      if (reader is null) { tbkReport.Text = "Error: Unable to read All   "; SystemSounds.Hand.Play(); return; }
      if (writer is null) { tbkReport.Text = "Error: Unable to read Sender"; SystemSounds.Hand.Play(); return; }

      var read = _ts.GetTargetTextFromWindow(reader ?? default);

      if (_prevReader == read) { tbkReport.Text = tbkStatus.Text = $"Same read  {DateTime.Now:ss}  {read.Length}"; SystemSounds.Hand.Play(); return; }

      _prevReader = read;
      if (_prevReader.Length < minLen) { tbkStatus.Text = $"Too Small  {DateTime.Now:ss}  '{read}'"; SystemSounds.Hand.Play(); return; }

      tbkReport.Text = tbkStatus.Text = $"Valid question!";

      var ary = read.Trim().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0; i < ary.Length; i++) { WriteLine($"{i,5})   {ary[i]}"); } // foreach (var ary2 in ary)WriteLine($"{i,3}) {ary2}");

      tbxPrompt.Text = ary[^1];

      if (IsAutoQrAI) QueryAI(1, new RoutedEventArgs());
      if (IsAutoType)
      {
        if (_prevMsgTpd != tbxPrompt.Text) //TypeMsg(1, new RoutedEventArgs());
        {
          WriteLine($"Typing   {tbxPrompt.Text}");
          var failMsg = _ts.SendMsg(writer ?? default, $"{tbxPrompt.Text}  {{ENTER}} ");
          if (string.IsNullOrEmpty(failMsg))
          {
            _prevMsgTpd = tbxPrompt.Text;
            tbkReport.Text = tbkStatus.Text = string.IsNullOrEmpty(failMsg) ? "Success typing in" : failMsg;
          }
          var failMs2 = _ts.SendMsg(writer ?? default, $"**");
        }
        else
          tbkReport.Text = tbkStatus.Text = "Same msg already typed in.";
      }
      else
        tbkReport.Text = tbkStatus.Text = "Auto type is OFF.";

      SystemSounds.Hand.Play();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); }
  }
  void OnCheckClipboardForData(string thread)
  {
    const int minLen = 10;
    try
    {
      if (!Clipboard.ContainsText() || _prevClpbrd == Clipboard.GetText()) { tbkStatus.Text = $"Same {DateTime.Now:ss}"; SystemSounds.Hand.Play(); return; }

      _prevClpbrd = Clipboard.GetText();
      if (_prevClpbrd.Length < minLen) { tbkStatus.Text = $"Too Small"; SystemSounds.Beep.Play(); SystemSounds.Hand.Play(); return; }

      tbkStatus.Text = $"Valid";
      tbxPrompt.Text = _prevClpbrd.Trim();

      if (IsAutoQrAI) QueryAI(1, new RoutedEventArgs());
      if (IsAutoType) TypeMsg(1, new RoutedEventArgs());

      SystemSounds.Hand.Play();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); }
  }

  public bool IsTimer_On { get; set; }
  public bool IsAutoQrAI { get; set; }
  public bool IsAutoType { get; set; }
  async void QueryAI(object s, RoutedEventArgs e)
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

      tbxPrompt.Focus();
      SetText(s, e);
    }
    finally
    {
      ((Control)s).IsEnabled = true;
    }
  }
  void SetText(object s, RoutedEventArgs e) { SystemSounds.Beep.Play(); _prevClpbrd = tbkAnswer.Text; Clipboard.SetText(tbkAnswer.Text); }
  void TypeMsg(object s, RoutedEventArgs e)
  {
    SystemSounds.Beep.Play();
    var rv = _ts.SendOnce(tbkAnswer.Text, _config["Ttl"]);
    WriteLine(tbkReport.Text = $"> SendKey result: {(rv.Length == 0 ? "Suceess!" : rv)}");
  }
  void ExitApp(object s, RoutedEventArgs e) { SystemSounds.Beep.Play(); Close(); }
}
// Tell me more about Ukraine.
