using System.Collections.ObjectModel;

namespace LogMonitorConsoleApp
{
  public class UserSettings : UserSettingsStore
  {
    public ObservableCollection<FileData> FileDataList { get; set; } = new();
    public string? TrgPath { get; set; }
  }
}