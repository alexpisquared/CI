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
    static readonly string _itaFile;
    readonly bool _ready = false;

    static IdleTimeoutAnalizer() => _itaFile= @$"RdpFacility.{Environment.MachineName}.ita.json";

    public static (IdleTimeoutAnalizer ita, string report) Create(DateTimeOffset started)
    {
      IdleTimeoutAnalizer ita;
      var report = "";
      if (File.Exists(_itaFile))
      {
        try
        {
          var jsonString = File.ReadAllText(_itaFile);
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
    [JsonIgnore] public bool ModeRO => Environment.GetCommandLineArgs().Length <= 1 || Environment.GetCommandLineArgs().Last().Contains("DevDbg");

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
      using var createStream = File.Create(_itaFile);
      await JsonSerializer.SerializeAsync(createStream, this);
    }
    void saveMe()
    {
      if (ModeRO)
        return;

      MinTimeoutMin = MinTimeout.TotalMinutes;
      var jsonString = JsonSerializer.Serialize(this);
      File.WriteAllText(_itaFile, jsonString);
    }

    internal void SaveLastClose()
    {
      LastClose = DateTimeOffset.Now;
      saveMe();
    }
  }
}
