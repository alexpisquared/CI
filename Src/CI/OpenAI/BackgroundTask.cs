﻿namespace OpenAI;
public class BackgroundTask //tu: timer - Scheduling repeating tasks with .NET 6’s NEW Timer - https://www.youtube.com/watch?v=J4JL4zR_l-0
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