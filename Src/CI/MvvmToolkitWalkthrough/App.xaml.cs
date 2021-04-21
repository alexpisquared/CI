using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using MvvmSample.Core.Services;
using MvvmSample.Core.ViewModels.Widgets;
using MvvmSampleXF.Services;
using MvvmToolkitWalkthrough.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmToolkitWalkthrough
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private bool _initialized;

    public App()
    {
      //??InitializeComponent();

      // Register services
      if (!_initialized)
      {
        _initialized = true;
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .AddSingleton<IFilesService, FileService>()
            .AddSingleton<ISettingsService, SettingsService>()
            .AddSingleton(RestService.For<IRedditService>("https://www.reddit.com/"))
            .BuildServiceProvider());
      }

      MainWindow = new MainWindow();
      MainWindow.Show();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
    }



    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
      var services = new ServiceCollection();

      services.AddSingleton<ISettingsService, SettingsService>();
      services.AddSingleton(RestService.For<IRedditService>("https://www.reddit.com/"));
      services.AddTransient<PostWidgetViewModel>();

      return services.BuildServiceProvider();
    }
  }
}
