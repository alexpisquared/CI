using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace RMSClient
{
  public partial class App : Application
  {
   public static DateTime _started = DateTime.Now;
    ILogger<RmsClientMainWindow> _logger;
    protected override void OnStartup(StartupEventArgs e)
    {
      _started = DateTime.Now;
      Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      var loggerFactory = LoggerFactory.Create(builder =>
      {
        var loggerConfiguration = new LoggerConfiguration()
          .WriteTo.File("Logs\\log.txt", rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information();

        builder.AddSerilog(loggerConfiguration.CreateLogger());
      });

      _logger = loggerFactory.CreateLogger<RmsClientMainWindow>();

      MainWindow = new RmsClientMainWindow(_logger);
      MainWindow.Show();

      base.OnStartup(e);      //dbIni: //DBInitializer.DropCreateDB();				//test: var _db = new MediaQADB();				_db.MediaInfos.Load();				foreach (var mi in _db.MediaInfos.Local) Console.WriteLine(mi); 
    }
    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - _started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }

    void Current_DispatcherUnhandledException(object s, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs ex)
    {
      if (ex != null)
        ex.Handled = true;

      var _header = $"Current Dispatcher Unhandled Exception - {DateTimeOffset.Now: HH:mm:ss}";
      var innerMsgs = $""
        + $"{ex?.Exception?.InnerException?.Message}  "
        + $"{ex?.Exception?.InnerException?.InnerException?.Message}  "
        + $"{ex?.Exception?.InnerException?.InnerException?.InnerException?.Message}  ";

      try
      {
        _logger.LogError($" +{(DateTime.Now - _started):mm\\:ss\\.ff}  CurrentDispatcherUnhandledException: s: {s?.GetType().Name}. {innerMsgs}");
        Clipboard.SetText(innerMsgs);
#if Speakable
        new System.Speech.Synthesis.SpeechSynthesizer().SpeakAsync($"Oopsee... {imex.Message}");
#endif
        if (Debugger.IsAttached)
        {
          Debugger.Break(); //seems like always true: if (ex is System.Windows.Threading.DispatcherUnhandledExceptionEventArgs)					Bpr.BeepEr();				else 
        }
        else if (MessageBox.Show($"An error occurred in this app...\n\n ...{innerMsgs}\n\nDo you want to continue?", _header, MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes) == MessageBoxResult.No)
        {
          _logger.LogInformation($" +{(DateTime.Now - _started):mm\\:ss\\.ff}  Decided NOT to continue: Application.Current.Shutdown();");
          Application.Current.Shutdown();
        }
      }
      catch (Exception fatalEx)
      {
        var msg = $"An error occurred while reportikng an error ...\n\n ...{fatalEx.Message}...\n\n ...{innerMsgs}";

        _logger.LogError($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {msg}");

        Environment.FailFast(msg, fatalEx); //tu: http://blog.functionalfun.net/2013/05/how-to-debug-silent-crashes-in-net.html // Capturing dump files with Windows Error Reporting: Db a key at HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\Windows Error Reporting\LocalDumps\[Your Application Exe FileName]. In that key, create a string value called DumpFolder, and set it to the folder where you want dumps to be written. Then create a DWORD value called DumpType with a value of 2.
        MessageBox.Show(msg, _header);

        throw;
      }
    }
  }
}
