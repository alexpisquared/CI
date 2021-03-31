using CI.GUI.Support.WpfLibrary.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CI.PermissionManager.Views;
using System.Windows;

namespace CI.PermissionManager
{
  public partial class App : Application
  {
    //PAsUsersSelectorWindow? _mainWindow;
    public static readonly DateTime Started;
    static readonly IConfigurationRoot _config;
    ILogger<PAsUsersSelectorWindow> _logger;

    static App()
    {
      Started = DateTime.Now;
      var aps = "appsettings.json";
      Again:
      try
      {
        _config = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile(aps)
          .AddUserSecrets<PAsUsersSelectorWindow>()
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
        if (!File.Exists(aps))
          File.WriteAllText(aps, @"
{
  ""WhereAmI"": "" ??\\PermMgrClient\\appsettings.json"",
  ""LogFolder"": ""Z:\\AlexPi\\Misc\\Logs\\PermMgr.aps.txt"",
  ""LogFolder2"": ""\\\\bbsfile01\\Public\\AlexPi\\Misc\\Logs\\PermMgr.aps.txt"",
  ""AppSettings"": {
    ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
    ""IpAddress"": ""10.10.19.152"",
    ""Port"": ""6756"",
    ""RmsDbConSt_"": ""Server=.\\sqlexpress;Database=RMS;Trusted_Connection=True;"",
    ""RmsDbConStr"": ""Server={0};Database={1};Trusted_Connection=True;"",
    ""Inv"": ""Server=MTdevSQLDB;Database=Inventory;Trusted_Connection=True;"",
    ""ForceSocketReconnect"": true,
    ""ForceSocketReconnectTime"": 300000,
    ""KeyVaultURL"": ""<moved to a safer place>""
  },
  ""VoiceNames"": [
    ""en-gb-george-apollo"",
    ""en-US-GuyNeural""
  ]
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

      _logger = loggerFactory.CreateLogger<PAsUsersSelectorWindow>();

      //todo: Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
      ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

      MainWindow = new PAsUsersSelectorWindow(_logger, _config);
      MainWindow.Show();

      base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }

    void onTogglePermission(object s, RoutedEventArgs e) => ((PAsUsersSelectorWindow)MainWindow)?.Recalc(s);
  }
}
