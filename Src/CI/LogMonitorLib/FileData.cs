namespace LogMonitorConsoleApp
{
  public class FileData
  {
    //public FileInfo? FileInfo { get; set; }
    public string FullName { get; set; } = "???";
    public DateTime LastWriteTime { get; set; } = DateTime.Now;
    public DateTime LastSeen { get; set; } = DateTime.Now;
    public string Status { get; set; } = "New";
    public bool IsDeleted { get; set; } = false;
  }
}