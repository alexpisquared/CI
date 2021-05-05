using CI.DS.Visual.Views;
using CI.Standard.Lib.Helpers;
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
    public  DateTime Started { get; private set; }
    readonly IConfigurationRoot _config;
    readonly ILogger<MainView> _logger;

    public App()
    {
      Started = DateTime.Now;
      _config = ConfigHelper.AutoInitConfig();
      _logger = SeriLogHelper.InitLoggerFactory(_config["LogFolder"] ?? "..\\Logs").CreateLogger<MainView>();

      var audit = $"** WhereAmI:  {_config?["WhereAmI"]}    {_config?["SqlConStr"]}    {_config?["LogFolder"] ?? "..\\Logs"}";

      Debug.WriteLine(audit);
      _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff}  {audit}");
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
      //Visual.Lib.Helpers.Bpr.Test();
      await Visual.Lib.Helpers.Bpr.Start();

      _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff}  App.OnStartup()");

      Current.DispatcherUnhandledException += UnhandledExceptionHndlr.OnCurrentDispatcherUnhandledException;

      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      MainWindow = new MainView(_logger, _config);
      MainWindow.Show();

      base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
      await Visual.Lib.Helpers.Bpr.Finish();
      _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff}  App.OnExit()\n"); base.OnExit(e);
    }
  }
}
