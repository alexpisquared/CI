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
    const string _fileName = @"IdleTimeoutAnalizer.json";
    readonly bool _ready = false;

    public static IdleTimeoutAnalizer LoadMe(DateTimeOffset started)
    {
      IdleTimeoutAnalizer ita;
      if (File.Exists(_fileName))
      {
        try
        {
          var jsonString = File.ReadAllText(_fileName);
          ita = JsonSerializer.Deserialize<IdleTimeoutAnalizer>(jsonString);
          ita.MinTimeout = TimeSpan.FromMinutes(ita.MinTimeoutMin);
          ita.ThisStart = started;
          ita.reCalc();
          return ita;
        }
        catch { }
      }

      ita = new IdleTimeoutAnalizer
      {
        LastClose = DateTimeOffset.MinValue,
        MinTimeout = TimeSpan.MaxValue,
        ThisStart = started
      };

      ita.reCalc();

      return ita;
    }


    public DateTimeOffset LastClose { get; set; }
    public double MinTimeoutMin { get; set; }
    [JsonIgnore]    public DateTimeOffset ThisStart { get; set; }
    [JsonIgnore] public TimeSpan MinTimeout { get; set; }

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
      if (Environment.GetCommandLineArgs().Last().Contains("DevDbg"))
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
