namespace OpenAI;
public partial class MainWindow : Window
{
  string _prevValue = "";
  IConfigurationRoot _config;
  CancellationTokenSource? _cts;

  public MainWindow()
  {
    InitializeComponent();
    _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build(); //var secretProvider = _config.Providers.First(); if (secretProvider.TryGet("WhereAmI", out var secretPass))  Console.WriteLine(secretPass);else  Console.WriteLine("Hello, World!");
  }

  void Window_Loaded(object sender, RoutedEventArgs e) { DeblockingTimer(); ; }
  void DeblockingTimer() => Task.Run(async () => await BlockingTimer());
  async Task BlockingTimer()
  {
    try
    {
      using PeriodicTimer tmr = new(TimeSpan.FromSeconds(1.5));
      _cts = new CancellationTokenSource();
      while (_cts is not null && await tmr.WaitForNextTickAsync(_cts.Token))
      {
        if (System.Windows.Application.Current.Dispatcher.CheckAccess()) // if on UI thread
          OnCheckClipboardForData();
        else
          await System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { OnCheckClipboardForData(); }));

        if (_cts?.Token.IsCancellationRequested == true) // Poll on this property if you have to do other cleanup before throwing.
        {
          WriteLine($"║   PeriodicTimer: -- CancellationRequested => Cancelling timer.");
          // Clean up here, then...
          _cts.Token.ThrowIfCancellationRequested();
        }
      }
    }
    catch (OperationCanceledException ex) { WriteLine($"║   PeriodicTimer: {ex.Message}"); }
    catch (Exception ex)             /**/ { WriteLine(ex.Message); }
    finally { _cts?.Dispose(); _cts = null; }
  }
  void OnCheckClipboardForData()
  {
    const int minLen = 10;
    try
    {
      if (!Clipboard.ContainsText() || _prevValue == Clipboard.GetText()) { SystemSounds.Hand.Play(); return; }

      _prevValue = Clipboard.GetText();
      if (_prevValue.Length < minLen) { SystemSounds.Beep.Play(); return; }

      SystemSounds.Hand.Play();

      tbxPrompt.Text = _prevValue;
    }
    catch (Exception ex) { WriteLine(ex.Message); }
  }

  void Send(object sender, RoutedEventArgs e)
  {
    var (ts, finishReason, answer) = OpenAILib.OpenAI.CallOpenAI(_config, 1250, tbxPrompt.Text);

    tbkAnswer.Text = answer;
    tbkTM.Text = $"{ts.TotalSeconds:N1}";
    tbkFR.Text = finishReason;
    tbkLn.Text = $"{answer.Length}";
    //tbkZZ.Text = rv.answer;

  }
  void Copy(object sender, RoutedEventArgs e)
  {
    _prevValue = tbkAnswer.Text;
    Clipboard.SetText(tbkAnswer.Text);
  }
  void Close(object sender, RoutedEventArgs e)
  {
    Close(); ;
  }
}
