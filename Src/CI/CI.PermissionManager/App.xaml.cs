﻿namespace CI.PermissionManager;

public partial class App : System.Windows.Application
{
  public static readonly DateTime Started;
  static readonly IConfigurationRoot _config;
  ILogger<PAsUsersSelectorWindow>? _logger;

  static App() // the one to base
  {
    Started = DateTime.Now;
    _config = ConfigHelper.AutoInitConfigFromFile(@"{{
  ""WhereAmI"": "" ??\\PermMgrClient\\appsettings.CI.PM.json  DFLT"",
  ""LogFolder"": ""\\\\bbsfile01\\Public\\Dev\\AlexPi\\Misc\\Logs\\PermMgr.DFLT..txt"",
  ""ServerList"": "".\\sqlexpress mtDEVsqldb mtUATsqldb"",
  ""SqlConStr"": ""Server={{0}};Database=Inventory;Trusted_Connection=True;"",
  ""AppSettings"": {{
    ""ServerList"": "".\\sqlexpress mtDEVsqldb mtUATsqldb"",
    ""RmsDbConStr"": ""Server={{0}};Database={{1}};Trusted_Connection=True;"",
    ""KeyVaultURL"": ""<moved to a safer place>""
  }}
}}");
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

    _logger = loggerFactory.CreateLogger<PAsUsersSelectorWindow>();

    //todo: Current.DispatcherUnhandledException += new RuntimeHelper(_logger, _config).Current_DispatcherUnhandledException;
    EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox ?? new TextBox()).SelectAll(); })); //tu: TextBox
    ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue)); //tu: ToolTip ShowDuration !!!

    new WindowManager().Arrange();

    MainWindow = new PAsUsersSelectorWindow(_logger, _config);
    MainWindow.Show(); //tu: use built-in MainWindow!!!!!!!!!!!!

    base.OnStartup(e);
  }
  protected override void OnExit(ExitEventArgs e) { _logger?.LogInformation($" +{(DateTime.Now - Started):mm\\:ss\\.ff} App.OnExit()          \n"); base.OnExit(e); }

  async void onTogglePermission(object s, RoutedEventArgs e) => await ((PAsUsersSelectorWindow)MainWindow).Recalc((FrameworkElement)s);
}
