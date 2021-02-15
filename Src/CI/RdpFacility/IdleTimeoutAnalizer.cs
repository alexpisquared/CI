using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RdpFacility
{
  public class IdleTimeoutAnalizer
  {
    static readonly string _fileName;
    readonly bool _ready = false;

    static IdleTimeoutAnalizer() => _fileName = @$"RdpFacility.IdleTimeoutAnalizer.{Environment.MachineName}.json";

    public static (IdleTimeoutAnalizer ita, string report) LoadMe(DateTimeOffset started)
    {
      IdleTimeoutAnalizer ita;
      var report = "";
      if (File.Exists(_fileName))
      {
        try
        {
          var jsonString = File.ReadAllText(_fileName);
          ita = JsonSerializer.Deserialize<IdleTimeoutAnalizer>(jsonString);
          ita.MinTimeout = TimeSpan.FromMinutes(ita.MinTimeoutMin);
          ita.ThisStart = started;
          ita.reCalc();
          return (ita, report);
        }
        catch (Exception ex) { report = ex.Message; }
      }

      ita = new IdleTimeoutAnalizer
      {
        LastClose = DateTimeOffset.MinValue,
        MinTimeout = TimeSpan.MaxValue,
        ThisStart = started
      };

      ita.reCalc();

      return (ita, report);
    }


    public DateTimeOffset LastClose { get; set; }
    public double MinTimeoutMin { get; set; }
    [JsonIgnore] public DateTimeOffset ThisStart { get; set; }
    [JsonIgnore] public TimeSpan MinTimeout { get; set; }
    [JsonIgnore] public bool ModeRO => Environment.GetCommandLineArgs().Count() <= 1 || Environment.GetCommandLineArgs().Last().Contains("DevDbg");

    public bool? IsAudible { get; set; }
    public bool? IsInsomnia { get; set; }

    void reCalc()
    {
      var thisTimeout = ThisStart - LastClose;
      if (MinTimeout == TimeSpan.Zero || MinTimeout > thisTimeout)
      {
        MinTimeout = thisTimeout;
        LastClose = DateTimeOffset.Now;
        saveMe();
      }
    }

    async Task saveMeAsync()
    {
      using var createStream = File.Create(_fileName);
      await JsonSerializer.SerializeAsync(createStream, this);
    }
    void saveMe()
    {
      if (ModeRO)
        return;

      MinTimeoutMin = MinTimeout.TotalMinutes;
      var jsonString = JsonSerializer.Serialize(this);
      File.WriteAllText(_fileName, jsonString);
    }

    internal void SaveLastClose()
    {
      LastClose = DateTimeOffset.Now;
      saveMe();
    }
  }
}
