using LogMonitorConsoleApp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace LogMonitorWpfApp
{
  public partial class MainWindow : Window
  {
    UserSettings _us;
    FileSystemWatcher _watcher;

    public MainWindow()
    {
      InitializeComponent();
      Topmost = Debugger.IsAttached;
    }

    void OnLoaded(object s, RoutedEventArgs e)
    {
      _us = UserSettingsStore.Load<UserSettings>();

      tbxPath.Text = Environment.GetCommandLineArgs().Length > 0 ? Environment.GetCommandLineArgs()[1] : @"Z:\Dev\alexPi\Misc\Logs";

      ReScanFolder(tbxPath.Text);

      _watcher = StartWatch(tbxPath.Text);

      dg1.ItemsSource = _us.FileDataList;
    }
    void OnScan(object s, RoutedEventArgs e) => ReScanFolder(tbxPath.Text);
    void OnWatch(object s, RoutedEventArgs e)
    {
      StopWatch();
      StartWatch(tbxPath.Text);
    }
    void OnClose(object s, RoutedEventArgs e) => Close();

    void ReScanFolder(string path)
    {
      var now = DateTime.Now;

      foreach (var fi in new DirectoryInfo(path).GetFiles().OrderByDescending(r => r.LastWriteTime))
      {
        //lb1.Items.Add($"\t{Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}");

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

      var del = _us.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) > 3);
      del.ToList().ForEach(fd => { fd.IsDeleted = true; fd.Status = "Deleted"; });
      var exi = _us.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) <= 3);
      exi.ToList().ForEach(fd => { fd.IsDeleted = false; });

      UserSettingsStore.Save(_us);

      dg1.Items.Refresh();
      
      Report($"Re-Scanned  {_us.FileDataList.Count}  files. {exi.Count()} + {del.Count()}.", "", "");      //foreach (var fi in _us.FileDataList.OrderByDescending(r => r.LastWriteTime))        lb1.Items.Add($"\t{System.IO.Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}  {fi.IsDeleted,-5}  {fi.Status}");
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

      Report($"··  Monitoring commnenced.  Path: {path}.   ", path, "");

      return watcher;
    }
    void StopWatch()
    {
      _watcher.Changed -= OnChanged;
      _watcher.Created -= OnCreated;
      _watcher.Deleted -= OnDeleted;
      _watcher.Renamed -= OnRenamed;
      _watcher.Error -= OnError;
    }

    void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (e.ChangeType != WatcherChangeTypes.Changed)
        return;

      ReportAndRescanSafe($"■■  Changed:  ", e.FullPath);
    }
    void OnCreated(object snr, FileSystemEventArgs e) => ReportAndRescanSafe($"██  Created:  ", e.FullPath);
    void OnDeleted(object snr, FileSystemEventArgs e) => ReportAndRescanSafe($"══  Deleted:  ", e.FullPath);
    void OnRenamed(object sender, RenamedEventArgs e) => ReportAndRescanSafe($"▄▀  Renamed:  ", e.OldFullPath, e.FullPath);
    void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());

    void PrintException(Exception? ex)
    {
      if (ex != null)
      {
        ReportAndRescanSafe($"████ Error: {ex.Message}", "");
        PrintException(ex.InnerException);
      }
    }
    void ReportAndRescanSafe(string msg, string file1, string file2 = "")
    {
      if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread
        ReportAndRescan(msg, file1, file2);
      else
        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => //todo: rejoin properly to the UI thread (Oct 2017)
        ReportAndRescan(msg, file1, file2)));
    }
    void ReportAndRescan(string msg, string file1, string file2)
    {
      Report(msg, file1, file2);
      ReScanFolder(tbxPath.Text);
    }

    void Report(string msg, string file1, string file2)
    {
      tb1.Text = $"{DateTimeOffset.Now:HH:mm}   {msg}           {Path.GetFileNameWithoutExtension(file1)}   {file2} \n";
      lb1.Items.Add($"{DateTimeOffset.Now:HH:mm}   {msg}           {Path.GetFileNameWithoutExtension(file1)}   {file2}");

      UseSayExe(msg);
    }

    void UseSayExe(string msg) => new Process { StartInfo = new ProcessStartInfo(@"Assets\say.exe", $"\"{msg}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
  }
}