﻿using CI.Standard.Lib.Helpers;
using CI.Standard.Lib.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;


using Ambience.Lib;
using CI.Standard.Lib.Helpers;
using CI.Standard.Lib.Services;
using CI.Visual.Lib.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace LogMonitorWpfApp
{
  public partial class App : System.Windows.Application
  {
    const string _sqlFormat = "SqlConStrSansSnD", _sfx = "Context";
    string _audit = "■ [not set] ■";
    readonly IServiceProvider _serviceProvider;
    readonly DateTime _appStarted;

    public App()
    {
      _appStarted = DateTime.Now;

      IServiceCollection services = new ServiceCollection();

      _ = services.AddSingleton<IConfigurationRoot>(ConfigHelper.AutoInitConfig());

      var logFile = $"LogMon.{Environment.UserName.Substring(0, 3)}";
      _ = services.AddSingleton<ILogger>(sp => SeriLogHelper.InitLoggerFactory(
        /*UserSettingsIPM.UserLogFolderFile ??= */FSHelper.GetCreateSafeLogFolderAndFile(new[]
        {
          sp.GetRequiredService<IConfigurationRoot>()["LogFolder"].Replace("..", $"{logFile}.."),
          @$"D:\Logs\CI.IPM\{logFile}",
          @$"\Temp\Logs\{logFile}",
          @$"\Logs\{logFile}",
          @$"..\Logs\{logFile}"
        }))
      .CreateLogger<MainWindow>());


      _ = VersionHelper.IsDevEnv ? services.AddTransient<IBpr, Ambience.Lib.Bpr>() : services.AddTransient<IBpr, BprSilentMock>();

      _ = services.AddTransient<IGTimer, GTimer>(sp => new GTimer(_appStarted));

      //_ = services.AddTransient<HomeLandingPage0VM>();
      //_ = services.AddTransient<UpdateViewCommand>();



      _ = services.AddSingleton<IAddChild, MainWindow>(); // (sp => new MainWindow(sp.GetRequiredService<ILogger>(), sp.GetRequiredService<IConfigurationRoot>(), sp.GetRequiredService<InventoryContext>(), _started));
      //_ = services.AddScoped<MainVM>();

      //services.AddSingleton<MainVM>();
      //services.AddSingleton<Window>(s => new MainWindow() { DataContext = s.GetRequiredService <MainVM>() });

      _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      try
      {
        UnhandledExceptionHndlr.Logger = _serviceProvider.GetRequiredService<ILogger>();
        Current.DispatcherUnhandledException += UnhandledExceptionHndlr.OnCurrentDispatcherUnhandledException;
        EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); }));
        ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

        _serviceProvider.GetRequiredService<IBpr>().Start();

        var cfg = _serviceProvider.GetRequiredService<IConfigurationRoot>();

        MainWindow = (Window)_serviceProvider.GetRequiredService<IAddChild>();
        MainWindow.Show();
        //MainWindow.DataContext = _serviceProvider.GetRequiredService<MainVM>();
        //await ((MainVM)MainWindow.DataContext).InitAsync();

        ShutdownMode = ShutdownMode.OnMainWindowClose;
      }
      catch (Exception ex)
      {
        _serviceProvider.GetRequiredService<ILogger>().LogError(ex, _audit);
        MessageBox.Show($"{ex.Message}\n\n{ex.StackTrace}\n\nPress OK to exit", "Fatal Error - unable to continue", MessageBoxButton.OK, MessageBoxImage.Error);
        Current.Shutdown();
      }

      base.OnStartup(e);
    }
    protected override void OnDeactivated(EventArgs e)
    {
      _serviceProvider.GetRequiredService<ILogger>().LogInformation($"+{(DateTimeOffset.Now - _appStarted).TotalHours:N2}h  App.OnDeactivated()  {_audit}");
      base.OnDeactivated(e);
    }
    protected override async void OnExit(ExitEventArgs e)
    {
      _serviceProvider.GetRequiredService<ILogger>().LogInformation($"+{(DateTimeOffset.Now - _appStarted).TotalHours:N2}h  App.OnExit()         {_audit}\n");
      base.OnExit(e);

      await _serviceProvider.GetRequiredService<IBpr>().FinishAsync();
    }
  }
}
