using System.Reflection.Metadata;
//using System.Windows.Forms;
using System.Windows.Interop;

namespace OpenAI;
public partial class MainWindow : Window
{
  const int _checkPeriodSec = 5;
  readonly IConfigurationRoot _config;
  readonly TextSender _ts = new TextSender();
  CancellationTokenSource? _cts;
  string _prevClpbrd = "", _prevReader = "";

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
        if (Application.Current is not null)
        {
          if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread
            CheckEndpoints("UI");
          else
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { CheckEndpoints("--"); }));
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
  void CheckEndpoints(string thread) { Write($" {thread} "); OnCheckTeamsForCahnges(); }
  void OnCheckTeamsForCahnges()
  {
    const int minLen = 10;
    try
    {
      var (reader, writer, whyFailed) = _ts.GetTwoWinndows(_config["Prc"], _config["Ttl"]);

      if (!string.IsNullOrEmpty(whyFailed)) { tbkReport.Text = whyFailed; return; }
      if (reader is null) { tbkReport.Text = "Error: Unable to read All   "; return; }
      if (writer is null) { tbkReport.Text = "Error: Unable to read Sender"; return; }

      var read = _ts.GetTargetTextFromWindow(reader ?? default);

      if (_prevReader == read) { tbkStatus.Text = $"Same read  {DateTime.Now:ss}"; return; }

      _prevReader = read;
      if (_prevReader.Length < minLen) { tbkStatus.Text = $"Too Small"; SystemSounds.Beep.Play(); return; }

      tbkReport.Text =
      tbkStatus.Text = $"Valid question!";

      var ary = read.Trim().Split(new char[] { '\r', '\n' });

      tbxPrompt.Text = ary[ary.Length - 5];

      if (IsAutoSend) QueryAI(1, new RoutedEventArgs());
      if (IsAutoText) TypeMsg(1, new RoutedEventArgs());

      var rv = (_ts.SendMsg(writer ?? default, $"{tbxPrompt.Text}  {{ENTER}}"));

      tbkReport .Text = String.IsNullOrEmpty(rv) ? "Success typing in" : rv;

      SystemSounds.Hand.Play();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); }
  }
  void OnCheckClipboardForData(string thread)
  {
    const int minLen = 10;
    try
    {
      if (!Clipboard.ContainsText() || _prevClpbrd == Clipboard.GetText()) { tbkStatus.Text = $"Same {DateTime.Now:ss}"; return; }

      _prevClpbrd = Clipboard.GetText();
      if (_prevClpbrd.Length < minLen) { tbkStatus.Text = $"Too Small"; SystemSounds.Beep.Play(); return; }

      tbkStatus.Text = $"Valid";
      tbxPrompt.Text = _prevClpbrd.Trim();

      if (IsAutoSend) QueryAI(1, new RoutedEventArgs());
      if (IsAutoText) TypeMsg(1, new RoutedEventArgs());

      SystemSounds.Hand.Play();
    }
    catch (Exception ex) { WriteLine(tbkReport.Text = ex.Message); }
  }

  public bool IsAutoSend { get; set; }
  public bool IsAutoText { get; set; }
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
