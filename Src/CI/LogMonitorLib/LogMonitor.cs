using CI.Standard.Lib.Helpers;
using Colorful;
using LogMonitorLib;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using CC = Colorful.Console;

namespace LogMonitorConsoleApp
{

  public class LogMonitor
  {
    readonly StyleSheet _styleSheet = new(Color.DarkGray);
    readonly UserSettings _us;
    FileSystemWatcher _watcher;

    public LogMonitor()
    {
      _us = UserSettings.Load;

      _styleSheet.AddStyle(".ale.", Color.LimeGreen);
      _styleSheet.AddStyle(".maz.", Color.Lime);
      _styleSheet.AddStyle(".sth.", Color.Lime);
      _styleSheet.AddStyle(".aau.", Color.Lime);
      _styleSheet.AddStyle(".cgi.", Color.Lime);
      _styleSheet.AddStyle(".mpa.", Color.Lime);
      _styleSheet.AddStyle(".hba.", Color.Lime);
      _styleSheet.AddStyle(".hsc.", Color.Lime);

      _styleSheet.AddStyle("True", Color.Green);
      _styleSheet.AddStyle("False", Color.Blue);

      _styleSheet.AddStyle("Created", Color.Green);
      _styleSheet.AddStyle("Deleted", Color.Red);
      _styleSheet.AddStyle("Renamed", Color.LightBlue);
      _styleSheet.AddStyle("Changed", Color.Yellow);
      _styleSheet.AddStyle("Error", Color.Orange);
      _styleSheet.AddStyle("[D,d]isabled", Color.Red);
      _styleSheet.AddStyle("(?i)CORPORATE", Color.LightBlue);
      _styleSheet.AddStyle("To exit - press any key.", Color.Cyan);
      _styleSheet.AddStyle("contains the following", Color.DarkCyan);

      _styleSheet.AddStyle("Still there + No changes", Color.DarkViolet);
      _styleSheet.AddStyle("Has been changed", Color.DarkCyan);

    }

    public void Start(string path)
    {
      CC.WriteLineStyled($"Neutral .maz. .sth. .hsc. Created Deleted Renamed Changed Error contains the following \n\n  {path}  contains the following:", _styleSheet);

      ReScanFolder(path);

      _watcher = StartWatch(path);

      while (true)
      {
        CC.Write(@$"
··  R-escan
··  To exit - press any key.
--  Json by VS Code
--  Json by Notepa 
--  Json by VS 2022
");

        switch (CC.ReadKey().Key)
        {
          case ConsoleKey.R: ReScanFolder(path); break;
          //case ConsoleKey.J: _ = new Process { StartInfo = new ProcessStartInfo(@"C:\Users\alexp\AppData\Local\Programs\Microsoft VS Code\Code.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); break;
          //case ConsoleKey.N: _ = new Process { StartInfo = new ProcessStartInfo(@"Notepad.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); break;
          //case ConsoleKey.V: _ = new Process { StartInfo = new ProcessStartInfo(@"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); break;
          default: return;
        }
      }
    }

    void ReScanFolder(string path)
    {
      var now = DateTime.Now;

      foreach (var fi in new DirectoryInfo(path).GetFiles().OrderByDescending(r => r.LastWriteTime))
      {
        //CC.WriteLineStyled($"\t{Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}", _styleSheet);

        var fd = _us.FileDataList.FirstOrDefault(r => r.FullName == fi.FullName);
        if (fd == null)
          _us.FileDataList.Add(new FileData { FullName = fi.FullName, LastWriteTime = fi.LastWriteTime });
        else
        {
          fd.LastSeen = now;
          if (Math.Abs((fd.LastWriteTime - fi.LastWriteTime).TotalSeconds) < 5)
            fd.Status = "Still there + No changes";
          else
          {
            fd.LastWriteTime = fi.LastWriteTime;
            fd.Status = $"Has been changed   at {fi.LastWriteTime}.";
          }
        }
      }

      _us.FileDataList.Where(r => r.LastSeen != now).ToList().ForEach(fd => { fd.IsDeleted = true; fd.Status = "Deleted"; });
      _us.FileDataList.Where(r => r.LastSeen == now).ToList().ForEach(fd => { fd.IsDeleted = false; });

      UserSettings.Save(_us);

      CC.WriteLine($"\t{"FullName",-40} {"LastWriteTime",-14}  {"Gone",-5}  {"Status"}", Color.DeepPink);

      foreach (var fi in _us.FileDataList.OrderByDescending(r => r.LastWriteTime))
        CC.WriteLineStyled($"\t{Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}  {fi.IsDeleted,-5}  {fi.Status}", _styleSheet);
    }

    FileSystemWatcher StartWatch(string path)
    {
      var watcher = new FileSystemWatcher(path)
      {
        NotifyFilter = NotifyFilters.Attributes
                           | NotifyFilters.CreationTime
                           | NotifyFilters.DirectoryName
                           | NotifyFilters.FileName
                           | NotifyFilters.LastAccess
                           | NotifyFilters.LastWrite
                           | NotifyFilters.Security
                           | NotifyFilters.Size
      };

      watcher.Changed += OnChanged;
      watcher.Created += OnCreated;
      watcher.Deleted += OnDeleted;
      watcher.Renamed += OnRenamed;
      watcher.Error += OnError;

      //watcher.Filter = "*.log";
      watcher.IncludeSubdirectories = true;
      watcher.EnableRaisingEvents = true;

      Report($"··  Monitoring commnenced.  Path: {path}.   ", path);

      return watcher;
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
      CC.Write($"{DateTimeOffset.Now:ddd HH:mm}   ", Color.DarkCyan);
      CC.WriteStyled($"{msg}           {Path.GetFileNameWithoutExtension(file1)}   {file2} \n", _styleSheet);
      UseSayExe(msg);
    }
    void UseSayExe(string msg) => new Process { StartInfo = new ProcessStartInfo(@"Assets\say.exe", $"\"{msg}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
  }
}