namespace OpenAI;

class PeriodicTimerInsideOfConstructor : IAsyncDisposable //todo: https://stackoverflow.com/questions/70688080/how-to-use-periodictimer-inside-of-constructor
{
  readonly CancellationTokenSource _cts = new();
  readonly PeriodicTimer _timer;
  readonly Task _timerTask;

  public PeriodicTimerInsideOfConstructor()
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