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
      _config = doit(@"C:\temp\appsettings.CI.DS.json");
    }
    static IConfigurationRoot doit(string aps)
    {
      try
      {
        for (var i = 0; i < 3; i++)
        {
          try
          {
            return new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile(aps)
              .AddUserSecrets<MainView>()
              .Build();
          }
          catch (InvalidOperationException ex)
          {
            ex.Pop(null, optl: "Disaster ...");
          }
          catch (Exception ex)
          {
            if (!tryToCreateDefaultFile(aps))
              ex.Pop(null, optl: "The default values will be used  ...maybe");
          }
        }

        throw new Exception($"Unable to create default  {aps}  file");
      }
      catch (Exception ex)
      {
        ex.Pop(null, optl: "The default values will be used  ...maybe");
      }

      throw new Exception($"Unable to create default  {aps}  file");
    }

    static bool tryToCreateDefaultFile(string aps)
    {
      try
      {
        if (!File.Exists(aps))
          File.WriteAllText(aps, @"
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

    protected override void OnStartup(StartupEventArgs e)
    {
      var loggerFactory = LoggerFactory.Create(builder =>
      {
        var loggerConfiguration = new LoggerConfiguration()
          .WriteTo.File(_config["LogFolder"], rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information();

        builder.AddSerilog(loggerConfiguration.CreateLogger());
      });

      _logger = loggerFactory.CreateLogger<MainView>();

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
