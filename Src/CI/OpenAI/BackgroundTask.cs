namespace OpenAI;
public class BackgroundTask //tu: timer - Scheduling repeating tasks with .NET 6’s NEW Timer - dhttps://www.youtube.com/watch?v=J4JL4zR_l-0
{
  readonly CancellationTokenSource _cts = new();
  readonly PeriodicTimer _timer;
  Task? _timerTask;
  public BackgroundTask(TimeSpan interval) => _timer = new PeriodicTimer(interval);
  
  public void Start(Action act) => _timerTask = DoWorkAsync(act);
  public async Task StopAsync()
  {
    if (_timerTask is null) return;

    _cts.Cancel();
    await _timerTask; // wait for the last invocation of the task to complete.
    _cts.Dispose();
    WriteLine("   ..Task (timer) was cancelled.");
  }

  async Task DoWorkAsync(Action act)
  {
    try
    {
      while (await _timer.WaitForNextTickAsync(_cts.Token))
      {
        act();
      }
    }
    catch (OperationCanceledException) { }
  }
}

class MyClass : IAsyncDisposable //todo: https://stackoverflow.com/questions/70688080/how-to-use-periodictimer-inside-of-constructor
{
  readonly CancellationTokenSource _cts = new();
  readonly PeriodicTimer _timer;
  readonly Task _timerTask;

  public MyClass()
  {
    _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    _timerTask = HandleTimerAsync(_timer, _cts.Token);
  }

  public void Cancel() => _cts.Cancel();

  async Task HandleTimerAsync(PeriodicTimer timer, CancellationToken cancel = default)
  {
    try
    {
      while (await timer.WaitForNextTickAsync(cancel))
      {
        await Task.Run(() => SomeHeavyJob(cancel), cancel);
      }
    }
    catch (Exception ex)
    {
      WriteLine($"..//Handle the exception but don't propagate it:\n\t{ex}");       
    }
  }

  void SomeHeavyJob(CancellationToken cancel) => throw new NotImplementedException();

  public async ValueTask DisposeAsync()
  {
    _timer.Dispose();
    await _timerTask;
    GC.SuppressFinalize(this);
  }
}