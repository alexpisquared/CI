using CI.DS.Visual.Views;
using CI.GUI.Support.WpfLibrary.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CI.DataSmarts
{
  public partial class App : Application
  {
    public static readonly DateTime Started;
    static readonly IConfigurationRoot _config;
    ILogger<MainView> _logger;

    static App() 
    {
      Started = DateTime.Now;
      _config = ConfigHelper.AutoInitConfig();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      _logger = SeriLogHelper.InitLoggerFactory(_config?["LogFolder"] ?? "..\\Logs").CreateLogger<MainView>();

      //todo: Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      MainWindow = new MainView(_logger, _config);
      MainWindow.Show();

      base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }
  }
}
