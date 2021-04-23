using Microsoft.Extensions.Logging;
using Serilog;

namespace CI.DataSmarts
{
  public class SeriLogHelper
  {
    public static ILoggerFactory InitLoggerFactory(string logFolder) => LoggerFactory.Create(builder =>
    {
      var loggerConfiguration = new LoggerConfiguration()
        .WriteTo.File(logFolder, rollingInterval: RollingInterval.Day)
        .MinimumLevel.Information();

      builder.AddSerilog(loggerConfiguration.CreateLogger());
    });
  }
}
