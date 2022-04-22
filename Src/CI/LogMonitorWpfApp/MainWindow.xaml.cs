using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CI.Standard.Lib.Base;
using CI.Standard.Lib.Helpers;
using LogMonitorConsoleApp;
using StandardContracts.Lib;

namespace LogMonitorWpfApp;

public partial class MainWindow : Window
{
  readonly FileSystemWatcher _watcher;
  readonly UserSettings _us;
  CancellationTokenSource? _ctsVideo, _ctsAudio, _ctsCheckr;
  const int _ms = 200;
  int _i = 0, _w = 0;

  public IBpr Bpr { get; }
  public MainWindow(IBpr bpr)
  {
    InitializeComponent();
    Bpr = bpr;
    Topmost = Debugger.IsAttached;
    MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };

    _us = UserSettings.Load;
    if (Environment.GetCommandLineArgs().Length > 1)
    {
      _us.TrgPath = Environment.GetCommandLineArgs()[1];
    }

    tbxPath.Text = _us.TrgPath;

    _ = ReScanFolder(tbxPath.Text);

    _watcher = new FileSystemWatcher(_us.TrgPath)
    {
      NotifyFilter = NotifyFilters.Attributes
                   | NotifyFilters.CreationTime
                   | NotifyFilters.DirectoryName
                   | NotifyFilters.FileName
                   | NotifyFilters.LastAccess
                   | NotifyFilters.LastWrite
                   | NotifyFilters.Security
                   | NotifyFilters.Size,
      IncludeSubdirectories = true,
      EnableRaisingEvents = true
    };

#if !_DEBUG
    if (Environment.MachineName == "D21-MJ0AWBEV") /**/ { Top = 32; Left = 920; }
    if (Environment.MachineName == "RAZER1")       /**/ { Top = 32; Left = 0; }
#endif
  }
  async void OnLoaded(object s, RoutedEventArgs e)
  {
    dg1.ItemsSource = _us.FileDataList;
    Title = $"Log Monitor - No events since  {DateTime.Now:HH:mm:ss}  -  {VersionHelper.CurVerStr}";
    StartWatch();
    await StartPeriodicChecker();
  }
  void OnChckFS(object s, RoutedEventArgs e)
  {
    tbkTitle.Text = $"{DateTimeOffset.Now:HH:mm:ss}  OnChckFS";
    Bpr.Tick();

    var rv = ReScanFolder(tbxPath.Text);
    if (rv != "")
    {
      ReportAndStartAlarms(rv, rv, null);
    }
  }

  async void OnReWtch(object s, RoutedEventArgs e) { await StopWatch(); await Bpr.TickAsync(); StartWatch(); }
  void OnExplre(object s, RoutedEventArgs e) { Bpr.Tick(); try { _ = new Process { StartInfo = new ProcessStartInfo(@"Explorer.exe", $"\"{tbxPath.Text}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); } catch (Exception ex) { MessageBox.Show(ex.ToString()); } }
  async void OnVSCode(object s, RoutedEventArgs e)
  {
    Bpr.Tick();
    await StopWatch();
    //WindowState = WindowState.Minimized;
    try
    {
      var process = new Process { StartInfo = new ProcessStartInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Programs\Microsoft VS Code\Code.exe", $"\"{tbxPath.Text}\"") { RedirectStandardError = true, UseShellExecute = false } };
      if (process.Start())
        process.WaitForExit();
    }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
    finally
    {
      StartWatch();
      WindowState = WindowState.Normal;
    }

    while (_ctsVideo is not null || _ctsAudio is not null) { _ctsVideo?.Cancel(); _ctsAudio?.Cancel(); }

  }
  void OnSetngs(object s, RoutedEventArgs e) { Bpr.Tick(); try { _ = new Process { StartInfo = new ProcessStartInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Programs\Microsoft VS Code\Code.exe", $"\"{UserSettingsStore.Store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); } catch (Exception ex) { MessageBox.Show(ex.ToString()); } }//_ = new Process { StartInfo = new ProcessStartInfo(@"Notepad.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); //_ = new Process { StartInfo = new ProcessStartInfo(@"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); }
  async void OnResetW(object s, RoutedEventArgs e)
  {
    await StopWatch();
    await Bpr.TickAsync();
    try
    {
      RemoveDeleteds();

      Title = $"Log Monitor  -  OnResetW on {DateTime.Now:HH:mm:ss}  -  {VersionHelper.CurVerStr}";
    }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
    finally { StartWatch(); }
  }
  async void OnMovOld(object s, RoutedEventArgs e)
  {
    Bpr.Tick();
    await StopWatch();

    try
    {
      foreach (var file in new DirectoryInfo(tbxPath.Text).GetFiles()) // var process = new Process { StartInfo = new ProcessStartInfo(@"CMD", $@"CMD /C MOVE {tbxPath.Text}\*.* {tbxPath.Text.Replace("Logs", "Logs.Old")} ") { RedirectStandardError = true, UseShellExecute = false } };      if (process.Start())        process.WaitForExit();
        if (file.LastWriteTime < DateTime.Today)
          File.Move(file.FullName, file.FullName.Replace("Logs", "Logs.Old"));

      OnChckFS(s, e);
      OnResetW(s, e);

      _ctsVideo?.Cancel();
      _ctsAudio?.Cancel();
      await Bpr.TickAsync();
      Background = System.Windows.Media.Brushes.Cyan;
      Title = $"Log Monitor  -  {VersionHelper.CurVerStr}  -  {DateTime.Now:HH:mm:ss} Moved Olds   ";
    }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
    finally { StartWatch(); }
  }
  async void OnAckAck(object s, RoutedEventArgs e)
  {
    Bpr.Tick();

    while (_ctsVideo is not null || _ctsAudio is not null) { _ctsVideo?.Cancel(); _ctsAudio?.Cancel();      /*await Bpr.BeepAsync(400, .4);*/    }

    WindowState = WindowState.Minimized;
    Background = System.Windows.Media.Brushes.DarkCyan;
    await Task.Delay(_ms * 10);
    Title = $"Log Monitor  -  {VersionHelper.CurVerStr}  -  {DateTime.Now:HH:mm:ss} minimized  * * * ";
    Topmost = false;
    await Bpr.TickAsync();
  }
  void On0000(object s, RoutedEventArgs e) { Bpr.Tick(); try { } catch (Exception ex) { MessageBox.Show(ex.ToString()); } }
  void OnClose(object s, RoutedEventArgs e) => Close();

  string ReScanFolder(string path)
  {
    var rv = "";
    try
    {
      var now = DateTime.Now;

      foreach (var fi in new DirectoryInfo(path).GetFiles().OrderByDescending(r => r.LastWriteTime))
      {
        var fd = _us.FileDataList.FirstOrDefault(r => r.FullName.Equals(fi.FullName, StringComparison.OrdinalIgnoreCase));
        if (fd == null)
        {
          _us.FileDataList.Add(new FileData { FullName = fi.FullName, LastWriteTime = fi.LastWriteTime, Status = "New" });
          rv += $" New: {fi.Name} ";
        }
        else
        {
          fd.LastSeen = now;
          if (Math.Abs((fd.LastWriteTime - fi.LastWriteTime).TotalSeconds) < 5)
            fd.Status = "No changes";
          else
          {
            fd.LastWriteTime = fi.LastWriteTime;
            fd.Status += $"+";// Changed   at {fi.LastWriteTime}.";
            rv += $" dTm: {fi.Name} ";
          }
        }
      }

      var del = _us.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) > 3); // if not seen above - mark as deleted
      del.ToList().ForEach(fd => { fd.IsDeleted = true; fd.Status = "Deleted"; rv += $" Del: {fd.PartName} "; });

      var exi = _us.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) <= 3); // if just seen - UN?deleted?
      exi.ToList().ForEach(fd => { fd.IsDeleted = false; }); //  fd.Status = "Restored"; rv += $" Exi: {Path.GetFileNameWithoutExtension(fd.FullName)} "; });

      RemoveDeleteds();

      _us.TrgPath = path;
      UserSettings.Save(_us);

      dg1.Items.Refresh();

      return rv; // $"Re-Scanned {_us.FileDataList.Count} files.  {del.Count()} deleted.";      //foreach (var fi in _us.FileDataList.OrderByDescending(r => r.LastWriteTime))        lb1.Items.Add($"\t{System.IO.Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}  {fi.IsDeleted,-5}  {fi.Status}");
    }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); return ex.Message; }
  }
  void StartWatch()
  {
    _watcher.Changed += OnChanged;
    _watcher.Created += OnCreated;
    _watcher.Deleted += OnDeleted;
    _watcher.Renamed += OnRenamed;
    _watcher.Error += OnError;

    tbkHeadr.Text = $" {++_w} ";
  }
  async Task StopWatch()
  {
    _watcher.Changed -= OnChanged;
    _watcher.Created -= OnCreated;
    _watcher.Deleted -= OnDeleted;
    _watcher.Renamed -= OnRenamed;
    _watcher.Error -= OnError;

    tbkHeadr.Text = $" {--_w} ";

    await Task.Delay(333);
  }

  void OnChanged(object s, FileSystemEventArgs e) { if (e.ChangeType == WatcherChangeTypes.Changed) ReportAndRescanSafe($"▼▲  Changed {File.GetLastWriteTime(e.FullPath).Second}. \t", e.FullPath); }
  void OnCreated(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"▲▲  Created.   \t", e.FullPath);
  void OnDeleted(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"▼▼  Deleted.   \t", e.FullPath);
  void OnRenamed(object sner, RenamedEventArgs e) => ReportAndRescanSafe($"►◄  Renamed.   \t", e.OldFullPath, e.FullPath);
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

    ReportAndStartAlarms(msg, file1, file2);
  }
  async void ReportAndStartAlarms(string msg, string changedFile, string file__Q)
  {
    tbkTitle.Text = $"{DateTimeOffset.Now:HH:mm:ss}  {msg}  {Path.GetFileNameWithoutExtension(changedFile)}  {file__Q}";
    lbxHist.Items.Add(tbkTitle.Text);

    WindowState = WindowState.Normal;
    Topmost = true;

    if (changedFile.Contains(".Er▄▀."))
    {
      await Task.Run(async () => await StartAudioNotifier(PlayErrorFAF));
      Background = System.Windows.Media.Brushes.Fuchsia;
    }
    else if (chkAll.IsChecked == true && _ctsAudio is null) // do not "hide" error sound!!!
    {
      await Task.Run(async () => await StartAudioNotifier(PlayQuietFAF));
      Background = System.Windows.Media.Brushes.Brown;
    }
    else
      Background = System.Windows.Media.Brushes.Yellow;

    await Task.Run(async () => await StartVisualNotifier());

#if !DEBUG
    UseSayExe(msg);
#endif
  }

  void PlayErrorFAF() => Task.Run(async () => await Bpr.WaveAsync(2000, 5000, 3));
  void PlayQuietFAF() => Task.Run(async () => await Bpr.WaveAsync(60, 401, 7)); //too quiet - worked on the old monitor speakers only: 060, 101, 7));

  async Task StartAudioNotifier(Action audio, [CallerMemberName] string? cmn = "")
  {
    Trace.WriteLine($"\n{DateTime.Now:HH:mm:ss}   Starting AUDIO by  {cmn} + + + + + + + + + + + + + + + ");
    _ctsAudio?.Cancel();
    _ctsAudio = new();
    PeriodicTimer timer = new(TimeSpan.FromMilliseconds(1000));
    try
    {
      while (await timer.WaitForNextTickAsync(_ctsAudio.Token))
      {
        audio();
        _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => tbkHeadr.Text = $" {_i++}++ "));          //await Task.Delay(_i);
      }
    }
    catch (OperationCanceledException ex) { Trace.WriteLine("Cancelled AUDIO:  - - - - - - - - - - - - - - - - - - " + ex.Message); }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
    finally { if (_ctsAudio is not null) { _ctsAudio.Dispose(); _ctsAudio = null; } }
  }
  async Task StartVisualNotifier([CallerMemberName] string? cmn = "")
  {
    Trace.WriteLine($"\n{DateTime.Now:HH:mm:ss}   Starting Visual by  {cmn} + + + + + + + + + + + + + + + ");
    _ctsVideo?.Cancel();
    _ctsVideo = new();
    PeriodicTimer timer = new(TimeSpan.FromMilliseconds(_ms + _ms));
    try
    {
      while (await timer.WaitForNextTickAsync(_ctsVideo.Token))
      {
        Trace.Write($"v");

        _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(async () =>
        {
          Title = $"▄▀▄▀▄▀▄▀   Log Monitor  -  {VersionHelper.CurVerStr}"; await Task.Delay(_ms);
          Title = $" ▄▀▄▀▄▀▄▀  Log Monitor  -  {VersionHelper.CurVerStr}"; await Task.Delay(_ms);
          Title = $"  ▄▀▄▀▄▀▄▀ Log Monitor  -  {VersionHelper.CurVerStr}"; await Task.Delay(_ms);
          Title = $"▀▄▀▄▀▄▀▄   Log Monitor  -  {VersionHelper.CurVerStr}"; await Task.Delay(_ms);
          Title = $" ▀▄▀▄▀▄▀▄  Log Monitor  -  {VersionHelper.CurVerStr}"; await Task.Delay(_ms);
          Title = $"  ▀▄▀▄▀▄▀▄ Log Monitor  -  {VersionHelper.CurVerStr}";
        }));
      }
    }
    catch (OperationCanceledException ex) { Trace.WriteLine("Cancelled Visual:  - - - - - - - - - - - - - - - - - - " + ex.Message); }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
    finally { if (_ctsVideo is not null) { _ctsVideo.Dispose(); _ctsVideo = null; } }
  }
  async Task StartPeriodicChecker()
  {
    Trace.WriteLine($"\n{DateTime.Now:HH:mm:ss}   Starting Checkr   ");
    _ctsCheckr?.Cancel();
    _ctsCheckr = new();
    PeriodicTimer timer = new(TimeSpan.FromSeconds(60));
    try
    {
      while (await timer.WaitForNextTickAsync(_ctsCheckr.Token))
      {
        Trace.Write($"C");
        OnChckFS(null, null);
      }
    }
    catch (OperationCanceledException ex) { Trace.WriteLine("Cancelled:  " + ex.Message); }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
    finally { if (_ctsCheckr is not null) { _ctsCheckr.Dispose(); _ctsCheckr = null; } }
  }

  void OnWtchOn(object sender, RoutedEventArgs e) => StartWatch();
  async void OnWtchNo(object sender, RoutedEventArgs e) => await StopWatch();
  async void OnStart6(object sender, RoutedEventArgs e) => await StartVisualNotifier();
  void OnStop_6(object sender, RoutedEventArgs e)
  {
    Trace.WriteLine($"\nCancelling  ({DateTime.Now:HH:mm:ss})");
    try
    {
      _ctsVideo?.Cancel();
      _ctsAudio?.Cancel();
      Trace.WriteLine($"Cancelled   both !!!!! ({DateTime.Now:HH:mm:ss})");
    }
    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
  }
  static void UseSayExe(string msg) => new Process { StartInfo = new ProcessStartInfo(@"say.exe", $"\"{msg}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
  void RemoveDeleteds()
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

}
