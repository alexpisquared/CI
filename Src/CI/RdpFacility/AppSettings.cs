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
        public bool IsInsmnia { get; set; }
        public bool IsPosning { get; set; }
        public bool IsMindBiz { get; set; }
        public double PeriodSec { get; set; } = 230; //240 - 10
        public double QuitAtHour { get; set; } = 18;

        static readonly string _stgFile;
        static AppSettings() => _stgFile = @$"RdpFacility.{Environment.MachineName}.stg.json";

        public static AppSettings Create()
        {
            try
            {
                if (File.Exists(_stgFile))
                    return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_stgFile)) ?? new AppSettings { IsInsmnia = true, IsAudible = true };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return new AppSettings { IsInsmnia = true, IsAudible = true };
        }

        public void Store() => File.WriteAllText(_stgFile, JsonSerializer.Serialize(this));
        public async Task StoreAsync() { using var createStream = File.Create(_stgFile); await JsonSerializer.SerializeAsync(createStream, this); }
    }
}
