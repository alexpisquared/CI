using System.Collections.ObjectModel;

namespace LogMonitorConsoleApp
{
  public class UserSettings : UserSettingsStore
  {
    public ObservableCollection<FileData> FileDataList { get; set; } = new();
  }
}