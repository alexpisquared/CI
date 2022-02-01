using CI.Standard.Lib.Base;
using CI.Standard.Lib.Helpers;
using System.Collections.ObjectModel;

namespace LogMonitorConsoleApp
{
  public class UserSettings : UserSettingsStore
  {
    public static UserSettings Load { get => Load<UserSettings>(); }
    public static void Save(UserSettings o) => Save<UserSettings>(o);

    public ObservableCollection<FileData> FileDataList { get; set; } = new();
    
    public string TrgPath { get; set; } = @"Z:\Dev\alexPi\Misc\Logs";
  }
}