﻿using CI.GUI.Support.WpfLibrary.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WinTiler.Views;

namespace WinTiler
{
  public partial class App : Application
  {
    public static readonly DateTime Started;
    static readonly IConfigurationRoot? _config;
    ILogger<TilerMainWindow>? _logger;

    static App()
    {
      Started = DateTime.Now;
      var aps = @"C:\temp\appsettings\WinTiler.json";
      Again:

      //MessageBox.Show("...Desperate measures  :) \n\n■ ■ ■ ■ ■ ■ ", "Desperate Times...");

      try
      {
        _config = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile(aps)
          .AddUserSecrets<TilerMainWindow>()
          .Build();
      }
      catch (Exception ex)
      {
        if (tryToCreateDefaultFile(aps))
          goto Again;

        ex.Pop(null, optl: "The default values will be used  ...maybe"); //  MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    static bool tryToCreateDefaultFile(string aps)
    {
      try
      {
        if (!Directory.Exists(Path.GetDirectoryName(aps)))
          Directory.CreateDirectory(Path.GetDirectoryName(aps));
        if (!File.Exists(aps))
          File.WriteAllText(aps, @"
{
  ""WhereAmI"": "" ??\\PermMgrClient\\appsettings.CI.PM.json  DFLT"",
  ""LogFolder"": ""\\\\bbsfile01\\Public\\AlexPi\\Misc\\Logs\\PermMgr.DFLT..txt"",
  ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
  ""SqlConStr"": ""Server={0};Database=Inventory;Trusted_Connection=True;"",
  ""AppSettings"": {
    ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
    ""RmsDbConStr"": ""Server={0};Database={1};Trusted_Connection=True;"",
    ""KeyVaultURL"": ""<moved to a safer place>""
  }
}");
        return true;
      }
      catch (Exception ex)
      {
        ex.Pop(null); //  MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        return false;
      }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      var loggerFactory = LoggerFactory.Create(builder =>
      {
        var loggerConfiguration = new LoggerConfiguration()
          .WriteTo.File(_config["LogFolder"], rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information();

        builder.AddSerilog(loggerConfiguration.CreateLogger());
      });

      _logger = loggerFactory.CreateLogger<TilerMainWindow>();

      //todo: Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      //new WindowManager().Arrange();

      MainWindow = new TilerMainWindow(_logger, _config);
      MainWindow.Show();

      base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }
    protected override void OnDeactivated(EventArgs e) { Debug.WriteLine($"{DateTime.Now:yy.MM.dd HH:mm:ss.f} +{(DateTime.Now - Started):mm\\:ss\\.ff} ▄▀▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▀▀▄▀App.OnDeactivated()  "); base.OnDeactivated(e); }
    protected override void OnActivated(EventArgs e) { Debug.WriteLine($"{DateTime.Now:yy.MM.dd HH:mm:ss.f} +{(DateTime.Now - Started):mm\\:ss\\.ff} ▄▀▄▄▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▄▀App.OnActivated()  "); base.OnActivated(e); ((TilerMainWindow)MainWindow).FindWindows(); }
  }
}
