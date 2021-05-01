using CI.DS.Visual.Views;
using CI.GUI.Support.WpfLibrary.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CI.DataSmarts
{
  public partial class App : Application
  {
    public readonly DateTime Started;
    readonly IConfigurationRoot _config;
    readonly ILogger<MainView> _logger;

    public App()
    {
      Started = DateTime.Now;
      _config = ConfigHelper.AutoInitConfig();
      _logger = SeriLogHelper.InitLoggerFactory(_config?["LogFolder"] ?? "..\\Logs").CreateLogger<MainView>();

      var audit =
        $"** Audit - WhereAmI:  {_config?["WhereAmI"] }  \r\n" +
        $"** Audit - SqlConSt:  {_config?["SqlConStr"]}  \r\n" +
        $"** Audit - LogFolde:  {_config?["LogFolder"]}  \r\n";
      
      Debug.WriteLine(audit);
      _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff}  {audit}");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff}  App.OnStartup()");

      //todo: Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      MainWindow = new MainView(_logger, _config);
      MainWindow.Show();

      base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff}  App.OnExit()          \n"); base.OnExit(e); }
  }
}
