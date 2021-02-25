using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RMSClient.Shared;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace RMSClient
{
  public partial class App : Application
  {
    static readonly IConfigurationRoot _config;
    public static DateTime _started = DateTime.Now;
    ILogger<RmsClientMainWindow> _logger;
    
    static App()
    {
      _config = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<RmsClientMainWindow>()
        .Build();
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
      _started = DateTime.Now;
      Application.Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      var loggerFactory = LoggerFactory.Create(builder =>
      {
        var loggerConfiguration = new LoggerConfiguration()
          .WriteTo.File(_config["LogFolder"], rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information();

        builder.AddSerilog(loggerConfiguration.CreateLogger());
      });

      _logger = loggerFactory.CreateLogger<RmsClientMainWindow>();

      MainWindow = new RmsClientMainWindow(_logger, _config);
      MainWindow.Show();

      base.OnStartup(e); 
    }
    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - _started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }
  }
}
