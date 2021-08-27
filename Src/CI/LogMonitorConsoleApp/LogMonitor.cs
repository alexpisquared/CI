namespace LogMonitorConsoleApp
{
  public class LogMonitor
  {
    public static void Start(string path = @"Z:\Dev\alexPi\Misc\Logs")
    {
      using var watcher = new FileSystemWatcher(path);

      watcher.NotifyFilter = NotifyFilters.Attributes
                           | NotifyFilters.CreationTime
                           | NotifyFilters.DirectoryName
                           | NotifyFilters.FileName
                           | NotifyFilters.LastAccess
                           | NotifyFilters.LastWrite
                           | NotifyFilters.Security
                           | NotifyFilters.Size;

      watcher.Changed += OnChanged;
      watcher.Created += OnCreated;
      watcher.Deleted += OnDeleted;
      watcher.Renamed += OnRenamed;
      watcher.Error += OnError;

      //watcher.Filter = "*.log";
      watcher.IncludeSubdirectories = true;
      watcher.EnableRaisingEvents = true;

      Console.WriteLine($"\a{DateTimeOffset.Now:ddd HH:mm}  Monitoring of  {path}  commnenced. Press enter to exit.");
      Console.ReadLine();
    }

    static void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (e.ChangeType != WatcherChangeTypes.Changed)
      {
        return;
      }

      Console.WriteLine($"\a{DateTimeOffset.Now:ddd HH:mm}  Changed: {e.FullPath}");
    }
    static void OnCreated(object snr, FileSystemEventArgs e) => Console.WriteLine($"\a{DateTimeOffset.Now:ddd HH:mm}  Created: {e.FullPath}");
    static void OnDeleted(object snr, FileSystemEventArgs e) => Console.WriteLine($"\a{DateTimeOffset.Now:ddd HH:mm}  Deleted: {e.FullPath}");
    static void OnRenamed(object sender, RenamedEventArgs e) => Console.WriteLine($"\a{DateTimeOffset.Now:ddd HH:mm}  Renamed:\n    Old: {e.OldFullPath}\n    New: {e.FullPath}");
    static void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());

    static void PrintException(Exception? ex)
    {
      if (ex != null)
      {
        Console.WriteLine($"\a{DateTimeOffset.Now:ddd HH:mm}  Message: {ex.Message}");
        Console.WriteLine("Stacktrace:");
        Console.WriteLine(ex.StackTrace);
        Console.WriteLine();
        PrintException(ex.InnerException);
      }
    }
  }
}