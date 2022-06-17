using Microsoft.Extensions.Hosting;

namespace TacticalSupport.Services;

public abstract class TimerScheduledService : BackgroundService // https://chowdera.com/2021/12/20211206205758914F.html
{
  readonly PeriodicTimer _timer;
  readonly TimeSpan _period;
  protected readonly ILogger Logger;
  protected TimerScheduledService(TimeSpan period, ILogger logger)
  {
    Logger = logger;
    _period = period;
    _timer = new PeriodicTimer(_period);
  }
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {

    try
    {

      while (await _timer.WaitForNextTickAsync(stoppingToken))

        try
        {

          Logger.LogInformation("Begin execute service");
          await ExecuteInternal(stoppingToken);
        }
        catch (Exception ex)
        {

          Logger.LogError(ex, "Execute exception");
        }
        finally
        {

          Logger.LogInformation("Execute finished");
        }
    }
    catch (OperationCanceledException operationCancelledException)
    {

      Logger.LogWarning(operationCancelledException, "service stopped");
    }
  }
  protected abstract Task ExecuteInternal(CancellationToken stoppingToken);
  public override Task StopAsync(CancellationToken cancellationToken)
  {

    Logger.LogInformation("Service is stopping.");
    _timer.Dispose();
    return base.StopAsync(cancellationToken);
  }
}

public class TimedHealthCheckService : TimerScheduledService
{
  public TimedHealthCheckService(ILogger<TimedHealthCheckService> logger) : base(TimeSpan.FromSeconds(5), logger)
  {

  }
  protected override Task ExecuteInternal(CancellationToken stoppingToken)
  {

    Logger.LogInformation("Executing...");
    return Task.CompletedTask;
  }
}
