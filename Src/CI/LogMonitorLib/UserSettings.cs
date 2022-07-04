namespace LogMonitorLib;

public class UserSettings : UserSettingsStore
{
  public static UserSettings Load => Load<UserSettings>();
  public static void Save(UserSettings o) => Save<UserSettings>(o);

  public ObservableCollection<FileData> FileDataList { get; set; } = new();

  public string TrgPat_ { get; set; } = @"Z:\Dev\alexPi\Misc\Logs";
  public string TrgPath { get; set; } = @"Z:\Dev\_Redis_MTDEV\CI.IPM\Logs";
}