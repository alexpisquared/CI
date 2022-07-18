namespace LogMonitorWpfApp;

public partial class App : System.Windows.Application
{
  readonly string _audit = "■ [not set] ■";
  readonly IServiceProvider _serviceProvider;
  readonly DateTime _appStarted;

  public App()
  {
    _appStarted = DateTime.Now;

    IServiceCollection services = new ServiceCollection();

    _ = services.AddSingleton<IConfigurationRoot>(ConfigHelper.AutoInitConfigFromFile());
    _ = services.AddSingleton<ILogger>(sp => SeriLogHelper.InitLoggerFactory(FSHelper.GetCreateSafeLogFolderAndFile(new[] { @$"..\Logs\" })).CreateLogger<TSMainWindow>());
    _ = services.AddTransient<IBpr, Bpr>();
    _ = services.AddTransient<IGTimer, GTimer>(sp => new GTimer(_appStarted));

    //_ = services.AddTransient<HomeLandingPage0VM>();
    //_ = services.AddTransient<UpdateViewCommand>();

    _ = services.AddSingleton<IAddChild, TSMainWindow>(); // (sp => new TSMainWindow(sp.GetRequiredService<ILogger>(), sp.GetRequiredService<IConfigurationRoot>(), sp.GetRequiredService<InventoryContext>(), _started));
    //_ = services.AddScoped<MainVM>();

    //services.AddSingleton<MainVM>();
    //services.AddSingleton<Window>(s => new TSMainWindow() { DataContext = s.GetRequiredService <MainVM>() });

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
      //TSMainWindow.DataContext = _serviceProvider.GetRequiredService<MainVM>();
      //await ((MainVM)TSMainWindow.DataContext).InitAsync();

      ShutdownMode = ShutdownMode.OnMainWindowClose;
    }
    catch (Exception ex)
    {
      _serviceProvider.GetRequiredService<ILogger>().LogError(ex, _audit);
      _ = MessageBox.Show($"{ex.Message}\n\n{ex.StackTrace}\n\nPress OK to exit", "Fatal Error - unable to continue", MessageBoxButton.OK, MessageBoxImage.Error);
      Current.Shutdown();
    }

    base.OnStartup(e);
  }
  //protected override void OnDeactivated(EventArgs e)    {      _serviceProvider.GetRequiredService<ILogger>().LogInformation($"+{(DateTimeOffset.Now - _appStarted).TotalHours:N2}h  App.OnDeactivated()  {_audit}");      base.OnDeactivated(e);    }
  protected override async void OnExit(ExitEventArgs e)
  {
    _serviceProvider.GetRequiredService<ILogger>().LogInformation($"+{(DateTimeOffset.Now - _appStarted).TotalHours:N2}h  App.OnExit()         {_audit}\n");
    base.OnExit(e);

    await _serviceProvider.GetRequiredService<IBpr>().FinishAsync();
  }
}
