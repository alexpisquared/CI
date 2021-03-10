using CI.GUI.Support.WpfLibrary.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RMSClient.Shared;
using Serilog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RMSClient
{
    public partial class App : Application
    {
        static readonly IConfigurationRoot _config;
        ILogger<RmsClientMainWindow> _logger;
        public static readonly DateTime Started;

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
                  .AddUserSecrets<RmsClientMainWindow>()
                  .Build();
            }
            catch (Exception ex)
            {
                if (tryToCreatedefaultFile(aps))
                    goto Again;

                ex.Pop(null, optl: "The default values will be used  ...maybe"); //  MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static bool tryToCreatedefaultFile(string aps)
        {
            try
            {
                if (!File.Exists(aps))
                    File.WriteAllText(aps, @"
{
  ""WhereAmI"": "" ??\\RMSClient\\appsettings.json"",
  ""LogFolder"": ""Z:\\AlexPi\\Misc\\Logs\\RMS.aps.txt"",
  ""LogFolder2"": ""\\\\bbsfile01\\Public\\AlexPi\\Misc\\Logs\\RMS.aps.txt"",
  ""AppSettings"": {
    ""IpAddress"": ""10.10.19.152"",
    ""Port"": ""6756"",
    ""RmsDbConStr"": ""Server=.\\sqlexpress;Database=RMS;Trusted_Connection=True;"",
    ""RmsDbConStr"": ""Server=MTdevSQLDB;Database=RMS;Trusted_Connection=True;"",
    ""BR"": ""Server=MTdevSQLDB;Database=BR;Trusted_Connection=True;"",
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

            _logger = loggerFactory.CreateLogger<RmsClientMainWindow>();

            Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox).SelectAll(); })); //tu: TextBox
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

            MainWindow = new RmsClientMainWindow(_logger, _config);
            MainWindow.Show();

            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e) { _logger.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }
    }
}
