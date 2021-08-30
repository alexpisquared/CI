using CI.Standard.Lib.Helpers;
using LogMonitorConsoleApp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LogMonitorWpfApp
{
  public partial class MainWindow : Window
  {
    readonly UserSettings _us;
    readonly FileSystemWatcher _watcher;

    public MainWindow()
    {
      InitializeComponent();
      Topmost = Debugger.IsAttached;
      MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };

      //return;
      try
      {
        _us = UserSettings.Load;
        _us.TrgPath = tbxPath.Text = Environment.GetCommandLineArgs().Length > 1 ? Environment.GetCommandLineArgs()[1] : @"Z:\Dev\alexPi\Misc\Logs";

        ReScanFolder(tbxPath.Text);

        _watcher = StartWatch(tbxPath.Text);
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); throw; }
    }

    void OnLoaded(object s, RoutedEventArgs e) => dg1.ItemsSource = _us.FileDataList;
    void OnScan(object s, RoutedEventArgs e) => Report(ReScanFolder(tbxPath.Text), "", "");
    void OnWatch(object s, RoutedEventArgs e) { StopWatch(); StartWatch(tbxPath.Text); }
    void OnClose(object s, RoutedEventArgs e) => Close();
    void OnClearHist(object sender, RoutedEventArgs e) => lb1.Items.Clear();

    string ReScanFolder(string path)
    {
      try
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
              fd.Status += $"+";// Changed   at {fi.LastWriteTime}.";
            }
          }
        }

        var del = _us.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) > 3);
        del.ToList().ForEach(fd => { fd.IsDeleted = true; fd.Status = "Deleted"; });

        var exi = _us.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) <= 3);
        exi.ToList().ForEach(fd => { fd.IsDeleted = false; });

        UserSettings.Save(_us);

        dg1.Items.Refresh();

        return $"Re-Scanned  {_us.FileDataList.Count}  files. {exi.Count()} + {del.Count()}.";      //foreach (var fi in _us.FileDataList.OrderByDescending(r => r.LastWriteTime))        lb1.Items.Add($"\t{System.IO.Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}  {fi.IsDeleted,-5}  {fi.Status}");
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); throw; }
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

      Report($"··  Monitoring commenced.  Path: {path}.   ", path, "");

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

    void OnChanged(object s, FileSystemEventArgs e)
    {
      if (e.ChangeType != WatcherChangeTypes.Changed)
        return;

      ReportAndRescanSafe($"■■  Changed:  ", e.FullPath);
    }
    void OnCreated(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"██  Created:  ", e.FullPath);
    void OnDeleted(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"══  Deleted:  ", e.FullPath);
    void OnRenamed(object sner, RenamedEventArgs e) => ReportAndRescanSafe($"▄▀  Renamed:  ", e.OldFullPath, e.FullPath);
    void OnError(object senderrr, ErrorEventArgs e) => ReportAnd_Exception(e.GetException());

    void ReportAnd_Exception(Exception? ex)
    {
      Bpr.ErrorFaF();
      if (ex != null)
      {
        ReportAndRescanSafe($"████ Error: {ex.Message}", "");
        ReportAnd_Exception(ex.InnerException);
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
      var fd = _us.FileDataList.FirstOrDefault(r => r.FullName == file1);
      if (fd != null)
        fd.Status = msg;

      Report(msg + ReScanFolder(tbxPath.Text), file1, file2);
    }

    void Report(string msg, string file1, string file2)
    {
      tb1.Text = $"{DateTimeOffset.Now:HH:mm}   {msg}           {Path.GetFileNameWithoutExtension(file1)}   {file2} \n";
      lb1.Items.Add($"{DateTimeOffset.Now:HH:mm}   {msg}           {Path.GetFileNameWithoutExtension(file1)}   {file2}");

      Bpr.TickFAF();

#if !DEBUG
      UseSayExe(msg);
#endif
    }

    void UseSayExe(string msg) => new Process { StartInfo = new ProcessStartInfo(@"say.exe", $"\"{msg}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();

  }
}