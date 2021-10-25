using Ambience.Lib;
using CI.Standard.Lib.Helpers;
using LogMonitorConsoleApp;
using StandardContracts.Lib;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LogMonitorWpfApp
{
  public partial class MainWindow : Window
  {
    readonly FileSystemWatcher _watcher;
    readonly DispatcherTimer _timerVisualNotifier;
    readonly UserSettings _us;
    const int _ms = 200;

    public IBpr Bpr { get; }

    public MainWindow(IBpr bpr)
    {
      InitializeComponent();
      Bpr = bpr;
      Topmost = Debugger.IsAttached;
      MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
      _timerVisualNotifier = new DispatcherTimer(TimeSpan.FromMilliseconds(_ms + _ms), DispatcherPriority.Background, new EventHandler(async (s, e) => { Title = $"▄▀▄▀▄▀▄▀ Log Monitor  -  {VersionHelper.CurVerStr}"; await Task.Delay(_ms); Title = $"▀▄▀▄▀▄▀▄ Log Monitor  -  {VersionHelper.CurVerStr}"; }), Dispatcher.CurrentDispatcher); //tu:

      _us = UserSettings.Load;
      if (Environment.GetCommandLineArgs().Length > 1)
      {
        _us.TrgPath = Environment.GetCommandLineArgs()[1];
      }

      tbxPath.Text = _us.TrgPath;

      tbkTitle.Text = ReScanFolder(tbxPath.Text);

      _watcher = StartWatch(tbxPath.Text);

      if (Environment.MachineName == "D21-MJ0AWBEV") /**/ { Top = 1608; Left = 1928; }
      if (Environment.MachineName == "RAZER1")       /**/ { Top = 1600; Left = 10; }
    }


    void OnLoaded(object s, RoutedEventArgs e) { dg1.ItemsSource = _us.FileDataList; OnStop(s, e); }
    void OnScan(object s, RoutedEventArgs e) => ReportAndStartAlarms(ReScanFolder(tbxPath.Text), "", "");
    void OnWatch(object s, RoutedEventArgs e) { Bpr.Tick(); StopWatch(); StartWatch(tbxPath.Text); }
    void OnStop(object s, RoutedEventArgs e) { Bpr.Tick(); /*lbxHist.Items.Clear();*/ _timerVisualNotifier.Stop(); Title = $"Log Monitor - No events since  {DateTime.Now:HH:mm}  -  {VersionHelper.CurVerStr}"; }
    void OnExpl(object s, RoutedEventArgs e) { Bpr.Tick(); try { _ = new Process { StartInfo = new ProcessStartInfo(@"Explorer.exe", $"\"{tbxPath.Text}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); } catch (Exception ex) { Trace.WriteLine(ex.Message); throw; } }
    void OnVScd(object s, RoutedEventArgs e) { Bpr.Tick(); try { _ = new Process { StartInfo = new ProcessStartInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Programs\Microsoft VS Code\Code.exe", $"\"{tbxPath.Text}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); } catch (Exception ex) { Trace.WriteLine(ex.Message); throw; } }
    void OnEdit(object s, RoutedEventArgs e) { Bpr.Tick(); try { _ = new Process { StartInfo = new ProcessStartInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Programs\Microsoft VS Code\Code.exe", $"\"{UserSettingsStore.Store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); } catch (Exception ex) { Trace.WriteLine(ex.Message); throw; } }//_ = new Process { StartInfo = new ProcessStartInfo(@"Notepad.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); //_ = new Process { StartInfo = new ProcessStartInfo(@"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); }
    void OnRDel(object s, RoutedEventArgs e)
    {
      Bpr.Tick();
      try
      {
        do
        {
          foreach (var deletedFile in _us.FileDataList.Where(r => r.IsDeleted))
          {
            _us.FileDataList.Remove(deletedFile);
            break;
          }
        } while (_us.FileDataList.Any(r => r.IsDeleted));
      }
      catch (Exception ex) { Trace.WriteLine(ex.Message); throw; }
    }
    void OnClose(object s, RoutedEventArgs e) => Close();
    void On0(object s, RoutedEventArgs e) { Bpr.Tick(); try { } catch (Exception ex) { Trace.WriteLine(ex.Message); throw; } }

    string ReScanFolder(string path)
    {
      try
      {
        var now = DateTime.Now;

        foreach (var fi in new DirectoryInfo(path).GetFiles().OrderByDescending(r => r.LastWriteTime))
        {
          var fd = _us.FileDataList.FirstOrDefault(r => r.FullName.Equals(fi.FullName, StringComparison.OrdinalIgnoreCase));
          if (fd == null)
            _us.FileDataList.Add(new FileData { FullName = fi.FullName, LastWriteTime = fi.LastWriteTime });
          else
          {
            fd.LastSeen = now;
            if (Math.Abs((fd.LastWriteTime - fi.LastWriteTime).TotalSeconds) < 5)
              fd.Status = "No changes";
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

        _us.TrgPath = path;
        UserSettings.Save(_us);

        dg1.Items.Refresh();

        return $"Re-Scanned {_us.FileDataList.Count} files.  {del.Count()} deleted.";      //foreach (var fi in _us.FileDataList.OrderByDescending(r => r.LastWriteTime))        lb1.Items.Add($"\t{System.IO.Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}  {fi.IsDeleted,-5}  {fi.Status}");
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); return ex.Message; }
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

      ReportAndStartAlarms($"Monitoring commenced.", path, "");

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

    void OnChanged(object s, FileSystemEventArgs e) { if (e.ChangeType == WatcherChangeTypes.Changed) ReportAndRescanSafe($"▼▲  Changed", e.FullPath); }
    void OnCreated(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"▲▲  Created", e.FullPath);
    void OnDeleted(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"▼▼  Deleted", e.FullPath);
    void OnRenamed(object sner, RenamedEventArgs e) => ReportAndRescanSafe($"►◄  Renamed", e.OldFullPath, e.FullPath);
    void OnError(object senderrr, ErrorEventArgs e) => ReportAnd_Exception(e.GetException());

    void ReportAnd_Exception(Exception? ex)
    {
      while (ex is not null)
      {
        ReportAndRescanSafe($"████ Error: {ex.Message}");
        ex = ex.InnerException;
      }

      Bpr.Error();
    }
    void ReportAndRescanSafe(string msg, string file1 = "", string file2 = "")
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

      ReportAndStartAlarms(msg + ReScanFolder(tbxPath.Text), file1, file2);
    }
    void ReportAndStartAlarms(string msg, string file1, string file2)
    {
      tbkTitle.Text = $"{DateTimeOffset.Now:HH:mm}  {msg}  {Path.GetFileNameWithoutExtension(file1)}  {file2}";
      lbxHist.Items.Add(tbkTitle.Text);

      Bpr.Tick();
      _timerVisualNotifier.Start();
      _timerVisualNotifier.IsEnabled = true;

      Task.Run(async () => await StartAnotherAlarm(file1.Contains(".Errs.")));

#if !DEBUG
      UseSayExe(msg);
#endif
    }
    async Task StartAnotherAlarm(bool isError)
    {
      while (_timerVisualNotifier.IsEnabled)
      {
        if (isError)
        {
          await Bpr.WaveAsync(2000, 5000, 3);
        }
        else
        {
          //await Bpr.WaveAsync(141, 100, 7);
          await Bpr.WaveAsync(060, 101, 7);
        }

        await Task.Delay(100);
      }
    }

    static void UseSayExe(string msg) => new Process { StartInfo = new ProcessStartInfo(@"say.exe", $"\"{msg}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
  }
}