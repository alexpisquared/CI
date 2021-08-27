using Colorful;
using System.Diagnostics;
using System.Drawing;
using CC = Colorful.Console;

namespace LogMonitorConsoleApp
{
  public class LogMonitor
  {
    readonly StyleSheet _styleSheet = new(Color.DarkGray);
    public LogMonitor()
    {
      _styleSheet.AddStyle("Created", Color.Lime);
      _styleSheet.AddStyle("Deleted", Color.Red);
      _styleSheet.AddStyle("Renamed", Color.LightBlue);
      _styleSheet.AddStyle("Changed", Color.Yellow);
      _styleSheet.AddStyle("Error", Color.Orange);
      _styleSheet.AddStyle("[D,d]isabled", Color.Red);
      _styleSheet.AddStyle("(?i)CORPORATE", Color.LightBlue);
      _styleSheet.AddStyle("Press enter to exit.", Color.Cyan);
      _styleSheet.AddStyle("contains the following", Color.DarkCyan);
    }

    public void Start(string path = @"Z:\Dev\alexPi\Misc\Logs")
    {
      Colorful.Console.WriteLineStyled($"WWWWWWWWWWW contains the following:", _styleSheet);
      CC.WriteLineStyled($"Created", _styleSheet);
      CC.WriteLineStyled($"Deleted", _styleSheet);
      CC.WriteLineStyled($"Renamed", _styleSheet);
      CC.WriteLineStyled($"Changed", _styleSheet);
      CC.WriteLineStyled($"Error", _styleSheet);
      
      CC.WriteLineStyled($"\n  {path}  contains the following:", _styleSheet);
      foreach (var file in Directory.GetFiles(path))
      {
        CC.WriteLineStyled($"\t {Path.GetFileName(file)}", _styleSheet);
      }

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

      Report($"··  Monitoring commnenced.  Path: {path}.    Press enter to exit.", path);

      CC.ReadLine();
    }

    void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (e.ChangeType != WatcherChangeTypes.Changed)
        return;

      Report($"■■  Changed:  ", e.FullPath);
    }
    void OnCreated(object snr, FileSystemEventArgs e) => Report($"██  Created:  ", e.FullPath);
    void OnDeleted(object snr, FileSystemEventArgs e) => Report($"══  Deleted:  ", e.FullPath);
    void OnRenamed(object sender, RenamedEventArgs e) => Report($"▄▀  Renamed:  ", e.OldFullPath, e.FullPath);
    void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());

    void PrintException(Exception? ex)
    {
      if (ex != null)
      {
        Report($"████████████████  Error: {ex.Message}", "");
        CC.WriteLineStyled($"\a{DateTimeOffset.Now:ddd HH:mm}  Message: {ex.Message}", _styleSheet);
        CC.WriteLineStyled("Stacktrace:", _styleSheet);
        CC.WriteLine(ex.StackTrace);
        CC.WriteLine();
        PrintException(ex.InnerException);
      }
    }
    void Report(string msg, string file1, string file2 = "")
    {
      CC.WriteLineStyled($"\a{DateTimeOffset.Now:ddd HH:mm}  {msg}           {Path.GetFileNameWithoutExtension(file1)}   {file2}", _styleSheet);
      UseSayExe(msg);
    }
    void UseSayExe(string msg) => new Process { StartInfo = new ProcessStartInfo(@"Assets\say.exe", $"\"{msg}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
  }
}