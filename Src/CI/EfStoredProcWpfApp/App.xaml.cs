using CI.GUI.Support.WpfLibrary.Helpers;
using EfStoredProcWpfApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EfStoredProcWpfApp
{
  public partial class App : Application
  {
    public static readonly DateTime Started;
    static readonly IConfigurationRoot? _config;
    ILogger<MainEfSpWindow>? _logger;

    static App()
    {
      Started = DateTime.Now;
      _config = ConfigHelper.AutoInitConfig();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      var loggerFactory = LoggerFactory.Create(builder =>
      {
        var loggerConfiguration = new LoggerConfiguration()
          .WriteTo.File(_config?["LogFolder"] ?? "..\\Logs", rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information();

        builder.AddSerilog(loggerConfiguration.CreateLogger());
      });

      _logger = loggerFactory.CreateLogger<MainEfSpWindow>();

      //todo: Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      new WindowManager().Arrange();

      MainWindow = new MainEfSpWindow(_logger, _config);
      MainWindow.Show(); //tu: use built-in MainWindow!!!!!!!!!!!!

      base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }

    //async void onTogglePermission(object s, RoutedEventArgs e) => await ((MainEfSpWindow)MainWindow)?.Recalc((FrameworkElement)s);
  }
}
