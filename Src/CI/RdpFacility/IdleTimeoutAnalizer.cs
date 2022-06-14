using System.Text.Json;
using System.Text.Json.Serialization;

namespace RdpSessionKeeper;

public class IdleTimeoutAnalizer
{
  static readonly string _itaFile;

  static IdleTimeoutAnalizer() => _itaFile = @$"RdpFacility.{Environment.MachineName}.ita.json";

  public static (IdleTimeoutAnalizer ita, string report) Create(DateTimeOffset started)
  {
    IdleTimeoutAnalizer ita;
    var report = "";
    if (File.Exists(_itaFile))
      try
      {
        var jsonString = File.ReadAllText(_itaFile);
        ita = JsonSerializer.Deserialize<IdleTimeoutAnalizer>(jsonString) ?? createDefault(started);
        ita.MinTimeout = TimeSpan.FromMinutes(ita.MinTimeoutMin);
        ita.ThisStart = started;
        ita.reCalc();
        return (ita, report);
      }
      catch (Exception ex) { report = ex.Message; }

    ita = createDefault(started);
    ita.reCalc();

    return (ita, report);
  }

  public DateTimeOffset LastClose { get; set; }
  public double MinTimeoutMin { get; set; }
  public bool SkipLoggingOnSelf { get; set; }
  public string Note { get; set; } = "";
  [JsonIgnore] public DateTimeOffset ThisStart { get; set; }
  [JsonIgnore] public TimeSpan MinTimeout { get; set; }
  [JsonIgnore] public bool RanByTaskScheduler => Environment.GetCommandLineArgs().Any(r => r.Contains("Task"));

  static IdleTimeoutAnalizer createDefault(DateTimeOffset started) => new() { LastClose = DateTimeOffset.MinValue, MinTimeout = TimeSpan.MaxValue, ThisStart = started };
  void reCalc()
  {
    var thisTimeout = ThisStart - LastClose;
    if (MinTimeout == TimeSpan.Zero || MinTimeout > thisTimeout)
    {
      MinTimeout = thisTimeout;
      LastClose = DateTimeOffset.Now;
      updateMeasureIfByTaskSchduler();
    }
  }

  async Task saveMeAsync()
  {
    using var createStream = File.Create(_itaFile);
    await JsonSerializer.SerializeAsync(createStream, this);
  }
  void updateMeasureIfByTaskSchduler()
  {
    if (RanByTaskScheduler)
    {
      MinTimeoutMin = MinTimeout.TotalMinutes;
      Note = $"{DateTimeOffset.Now}  Updated <= ran by TaskSch-r.";
    }
    else
      Note = $"{DateTimeOffset.Now}  Keeping the same <= not ran by TaskSch-r (...or it'd be {MinTimeout})";

    File.WriteAllText(_itaFile, JsonSerializer.Serialize(this));
  }

  internal void SaveLastCloseAndAnalyzeIfMarkable()
  {
    LastClose = DateTimeOffset.Now;
    updateMeasureIfByTaskSchduler();
  }
}
