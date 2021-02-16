using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RdpFacility
{
  class AppSettings
  {
    public bool IsAudible { get; set; }
    public bool IsInsomnia { get; set; }
    public bool IsMousing { get; set; }

    static readonly string _stgFile;
    static AppSettings() => _stgFile = @$"RdpFacility.{Environment.MachineName}.stg.json";

    public static AppSettings Create()
    {
      if (File.Exists(_stgFile))
        try { return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_stgFile)); } catch (Exception ex) { Debug.WriteLine(ex); }

      return new AppSettings { IsInsomnia = true, IsAudible = true };
    }

    public void Store() => File.WriteAllText(_stgFile, JsonSerializer.Serialize(this)); // async Task saveMeAsync()    {      using var createStream = File.Create(_stgFile);      await JsonSerializer.SerializeAsync(createStream, this);    }
  }
}
