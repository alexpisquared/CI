using CI.DS.Visual.Views;
using CI.GUI.Support.WpfLibrary.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CI.DataSmarts
{
  public partial class App : Application
  {
    public static readonly DateTime Started;
    static readonly IConfigurationRoot _config;
    ILogger<MainView> _logger;

    static App() // the one to base
    {
      Started = DateTime.Now;
      _config = StartUpHelper.InitConfig(@"C:\temp\appsettings.CI.DS.json");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      _logger = StartUpHelper.InitLoggerFactory(_config["LogFolder"]).CreateLogger<MainView>();

      //todo: Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      MainWindow = new MainView(_logger, _config);
      MainWindow.Show();

      base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }
  }

  public class StartUpHelper
  {
    public static IConfigurationRoot InitConfig(string appsettingsFile)
    {
      try
      {
        for (var i = 0; i < 3; i++)
        {
          try
          {
            return new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile(appsettingsFile)
              .AddUserSecrets<MainView>()
              .Build();
          }
          catch (InvalidOperationException ex)
          {
            ex.Pop(null, optl: "Disaster ...");
          }
          catch (Exception ex)
          {
            if (!tryToCreateDefaultFile(appsettingsFile))
              ex.Pop(null, optl: "The default values will be used  ...maybe");
          }
        }

        throw new Exception($"Unable to create default  {appsettingsFile}  file");
      }
      catch (Exception ex)
      {
        ex.Pop(null, optl: "The default values will be used  ...maybe");
      }

      throw new Exception($"Unable to create default  {appsettingsFile}  file");
    }
    public static ILoggerFactory InitLoggerFactory(string logFolder) => LoggerFactory.Create(builder =>
    {
      var loggerConfiguration = new LoggerConfiguration()
        .WriteTo.File(logFolder, rollingInterval: RollingInterval.Day)
        .MinimumLevel.Information();

      builder.AddSerilog(loggerConfiguration.CreateLogger());
    });

    static bool tryToCreateDefaultFile(string appsettingsFile)
    {
      try
      {
        if (!File.Exists(appsettingsFile))
          File.WriteAllText(appsettingsFile, @"
{
  ""WhereAmI"": "" ??\\PermMgrClient\\appsettings.CI.PM.json  DFLT"",
  ""LogFolder"": ""\\\\bbsfile01\\Public\\AlexPi\\Misc\\Logs\\PermMgr.DFLT..txt"",
  ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
  ""SqlConStr"": ""Server=MTdevSQLDB;Database=Inventory;Trusted_Connection=True;"",
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
        ex.Pop(null);
        return false;
      }
    }
  }
}
