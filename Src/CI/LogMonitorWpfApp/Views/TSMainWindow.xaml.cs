namespace LogMonitorWpfApp;
public partial class TSMainWindow : Window
{
  readonly FileSystemWatcher _watcher;
  readonly UserSettings _userSettingsAndStateOfFS;
  readonly IBpr _bpr;
  CancellationTokenSource? _ctsVideo, _ctsAudio, _ctsCheckr;
  const int _200ms = 200;
  const string _noChanges = "No changes";
  int _i = 0, _w = 0, _v = 0, _a = 0;
  readonly Index _4thFromEnd = ^4;

  public TSMainWindow(IBpr bpr)
  {
    InitializeComponent();
    rsk.Bpr = _bpr = bpr;
    Topmost = Debugger.IsAttached;
    MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };

    _userSettingsAndStateOfFS = UserSettings.Load;
    if (Environment.GetCommandLineArgs().Length > 1)
    {
      _userSettingsAndStateOfFS.TrgPath = Environment.GetCommandLineArgs()[1];
    }

    tbxPath.Text = _userSettingsAndStateOfFS.TrgPath;

    _ = ReScanFolder_SetCurrentStateToWatchChangesAgainst(tbxPath.Text); // resets the mark.

    _watcher = new FileSystemWatcher(_userSettingsAndStateOfFS.TrgPath)
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
    if (Environment.MachineName == "D21-MJ0AWBEV") /**/ { Top = 32; Left = 880; }

    if (Environment.MachineName == "RAZER1")       /**/ { Top = 32; Left = 0; }
#endif
  }
  async void OnLoaded(object s, RoutedEventArgs e)
  {
    dg1.ItemsSource = _userSettingsAndStateOfFS.FileDataList;
    Title = $"{DateTime.Now:HH:mm}  No events since  ..  {VersionHelper.CurVerStrYMd}";
    StartWatch();
    await StartPeriodicChecker();
  }
  void OnChckFS(object s, RoutedEventArgs e) { _bpr.Click(); ChckFS(); }
  void OnExplre(object s, RoutedEventArgs e) { _bpr.Click(); try { _ = new Process { StartInfo = new ProcessStartInfo(@"Explorer.exe", $"\"{tbxPath.Text}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); } catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); } }
  async void OnVSCode(object s, RoutedEventArgs e)
  {
    _bpr.Click();
    await StopWatch();
    WindowState = WindowState.Minimized;
    try
    {
      var process = new Process { StartInfo = new ProcessStartInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Programs\Microsoft VS Code\Code.exe", $"\"{tbxPath.Text}\"") { RedirectStandardError = true, UseShellExecute = false } };
      if (process.Start())
        process.WaitForExit(); // does not hold the execution ... but only when multiple instances running !?!?!?!?!
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }
    finally
    {
      _ = ReScanFolder_SetCurrentStateToWatchChangesAgainst(tbxPath.Text); // resets the mark.
      StartWatch();
      WindowState = WindowState.Normal;
    }

    while (_ctsVideo is not null || _ctsAudio is not null) { _ctsVideo?.Cancel(); _ctsAudio?.Cancel(); }
  }
  void OnSetngs(object s, RoutedEventArgs e) { _bpr.Click(); try { _ = new Process { StartInfo = new ProcessStartInfo(@$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Programs\Microsoft VS Code\Code.exe", $"\"{UserSettingsStore.FullPath}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); } catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); } }//_ = new Process { StartInfo = new ProcessStartInfo(@"Notepad.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); //_ = new Process { StartInfo = new ProcessStartInfo(@"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe", $"\"{UserSettingsStore._store}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start(); }
  async void OnResetW(object s, RoutedEventArgs e)
  {
    await StopWatch();
    await _bpr.ClickAsync();
    try
    {
      RemoveDeleteds();

      Title = $"{DateTime.Now:HH:mm}  OnResetW on  ..  {VersionHelper.CurVerStrYMd}";
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }
    finally { StartWatch(); }
  }
  async void OnMovOld(object s, RoutedEventArgs e)
  {
    _bpr.Click();
    await StopWatch();

    try
    {
      var trgOldFolder = Directory.CreateDirectory($"{tbxPath.Text}.Old");

      foreach (var srcLogFolder in new[] { tbxPath.Text, @"Z:\Dev\AlexPi\Misc\Logs" })
      {
        foreach (var logFileInfo in new DirectoryInfo(srcLogFolder).GetFiles().Where(fi => fi.LastWriteTime < DateTime.Today)) // var process = new Process { StartInfo = new ProcessStartInfo(@"CMD", $@"CMD /C MOVE {srcLogFolder}\*.* {srcLogFolder.Replace("Logs", "Logs.Old")} ") { RedirectStandardError = true, UseShellExecute = false } };      if (process.Start())        process.WaitForExit();
        {
          var trg = Path.Combine(trgOldFolder.FullName, logFileInfo.Name);
          var nm0 = Path.GetFileNameWithoutExtension(trg);
          for (var i = 0; File.Exists(trg) && i < 999; i++)
          {
            trg = Path.Combine(Path.GetDirectoryName(trg)!, $"{nm0}.{i}") + Path.GetExtension(trg);
          }

          File.Move(logFileInfo.FullName, trg);
        }
      }

      ChckFS(true);
      OnResetW(s, e);

      _ctsVideo?.Cancel();
      _ctsAudio?.Cancel();
      await _bpr.TickAsync();
      brdr1.Background = Brushes.Cyan;
      Title = $"{DateTime.Now:HH:mm}  Olds moved  ..  {VersionHelper.CurVerStrYMd}  ";
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }
    finally { StartWatch(); }
  }
  async void OnAckAck(object s, RoutedEventArgs e) => await AckAck(s);

  private async Task AckAck(object s)
  {
    Title = $"Ack...";
    _bpr.Click();

    while (_ctsVideo is not null || _ctsAudio is not null) { _ctsVideo?.Cancel(); _ctsAudio?.Cancel(); await _bpr.BeepAsync(200, .333); }

    if (s is Button)
      WindowState = WindowState.Minimized;

    brdr1.Background = Brushes.DarkCyan;
    //?await Task.Delay(_200ms * 8); 
    Topmost = false;
    await _bpr.TickAsync();
    Title = $"{DateTime.Now:HH:mm}  Minimized  ..  {VersionHelper.CurVerStrYMd}  ";
  }

  void On0000(object s, RoutedEventArgs e) { _bpr.Click(); try { } catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); } }
  void OnClose(object s, RoutedEventArgs e) => Close();

  void ChckFS(bool skipReporting = false)
  {
    tbkTitle.Text = $"{DateTimeOffset.Now:HH:mm:ss}  ChckFS";
    var report = ReScanFolder_SetCurrentStateToWatchChangesAgainst(tbxPath.Text);
    if (report != _noChanges && !skipReporting)
      ReportAndStartAlarms("By FS Check", report);
  }
  string ReScanFolder_SetCurrentStateToWatchChangesAgainst(string path)
  {
    var report = _noChanges;
    try
    {
      var now = DateTime.Now;

      foreach (var fi in new DirectoryInfo(path).GetFiles().OrderByDescending(r => r.LastWriteTime))
      {
        var fd = _userSettingsAndStateOfFS.FileDataList.FirstOrDefault(r => r.FullName.Equals(fi.FullName, StringComparison.OrdinalIgnoreCase));
        if (fd == null)
        {
          _userSettingsAndStateOfFS.FileDataList.Add(new FileData { FullName = fi.FullName, LastWriteTime = fi.LastWriteTime, Status = "New", LengthKb = new FileInfo(fi.FullName).Length / 1000 });
          report += $" New: {fi.Name} ";
        }
        else
        {
          fd.LastSeen = now;
          if (Math.Abs((fd.LastWriteTime - fi.LastWriteTime).TotalSeconds) < 5)
          {
            fd.Status = _noChanges;
            fd.LengthKb = new FileInfo(fi.FullName).Length / 1000;
          }
          else
          {
            fd.LastWriteTime = fi.LastWriteTime;
            fd.LengthKb = new FileInfo(fi.FullName).Length / 1000;
            fd.Status += $"+";// Changed   at {fi.LastWriteTime}.";
            report += $" dTm: {fi.Name} ";
          }
        }
      }

      var del = _userSettingsAndStateOfFS.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) > 3); // if not seen above - mark as deleted
      del.ToList().ForEach(fd => { fd.IsDeleted = true; fd.Status = "Deleted"; report += $" Del: {fd.PartName} "; });

      var exi = _userSettingsAndStateOfFS.FileDataList.Where(r => Math.Abs((r.LastSeen - now).TotalSeconds) <= 3); // if just seen - UN?deleted?
      exi.ToList().ForEach(fd => { fd.IsDeleted = false; }); //  fd.Status = "Restored"; report += $" Exi: {Path.GetFileNameWithoutExtension(fd.FullName)} "; });

      RemoveDeleteds();

      _userSettingsAndStateOfFS.TrgPath = path;
      UserSettings.Save(_userSettingsAndStateOfFS);

      dg1.Items.SortDescriptions.Clear();
      dg1.Items.SortDescriptions.Add(new SortDescription("LastWriteTime", ListSortDirection.Descending));
      dg1.Items.Refresh();

      return report; // $"Re-Scanned {_userSettingsAndStateOfFS.FileDataList.Count} files.  {del.Count()} deleted.";      //foreach (var fi in _userSettingsAndStateOfFS.FileDataList.OrderByDescending(fi => fi.LastWriteTime))        lb1.Items.Add($"\t{System.IO.Path.GetFileName(fi.FullName),-40} {fi.LastWriteTime:MM-dd HH:mm:ss}  {fi.IsDeleted,-5}  {fi.Status}");
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); return ex.Message; }
  }
  void StartWatch([CallerMemberName] string? cmn = "")
  {
    WriteLine($"\n{DateTime.Now:HH:mm:ss}   Starting  FS WATCH  by  {cmn} {new string('+', 64)}");
    _watcher.Changed += OnChanged;
    _watcher.Created += OnCreated;
    _watcher.Deleted += OnDeleted;
    _watcher.Renamed += OnRenamed;
    _watcher.Error += OnError;

    tbkHeadr.Text = $" {++_w} {_v} {_a}";
  }
  async Task StopWatch([CallerMemberName] string? cmn = "")
  {
    WriteLine($"\n{DateTime.Now:HH:mm:ss}   Stoppping FS WATCH  by  {cmn} {new string('-', 64)}");
    _watcher.Changed -= OnChanged;
    _watcher.Created -= OnCreated;
    _watcher.Deleted -= OnDeleted;
    _watcher.Renamed -= OnRenamed;
    _watcher.Error -= OnError;

    tbkHeadr.Text = $" {--_w} {_v} {_a} ";

    await Task.Delay(333);
  }

  void OnChanged(object s, FileSystemEventArgs e) { if (e.ChangeType == WatcherChangeTypes.Changed) ReportAndRescanSafe($"▼▲  Changed. \t", e.FullPath); }
  void OnCreated(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"▲▲  Created. \t", e.FullPath);
  void OnDeleted(object s, FileSystemEventArgs e) => ReportAndRescanSafe($"▼▼  Deleted. \t", e.FullPath);
  void OnRenamed(object sner, RenamedEventArgs e) => ReportAndRescanSafe($"►◄  Renamed. \t", e.FullPath);
  void OnError(object senderrr, ErrorEventArgs e) => ReportAnd_Exception(e.GetException());

  void ReportAnd_Exception(Exception? ex)
  {
    while (ex is not null)
    {
      ReportAndRescanSafe($"████ Error: {ex.Message}");
      ex = ex.InnerException;
    }

    _bpr.Error();
  }
  void ReportAndRescanSafe(string msg, string file1 = "")
  {
    var d = file1.Split('.');
    var rpt =
      (d.Length > 4) ? $"{msg} {d[_4thFromEnd].ToUpper()}" :
      (d.Length > 2) ? $"{msg} {d[2].ToUpper()}" :
      $"{msg}  {Path.GetFileNameWithoutExtension(file1)}";

    RunUIThreadSafe(ReportAndRescan, rpt, file1);
  }
  static void RunUIThreadSafe(Action<string, string> action, string report, string filename)
  {
    if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread
      action(report, filename);
    else
      _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => action(report, filename))); //tu: rejoin properly to the UI thread (Oct 2017)
  }
  void ReportAndRescan(string msg, string file1)
  {
    var fd = _userSettingsAndStateOfFS.FileDataList.FirstOrDefault(r => r.FullName == file1);
    if (fd != null)
      fd.Status = msg;

    ReportAndStartAlarms(msg, file1);
  }
  async void ReportAndStartAlarms(string msg, string changedFile)
  {
    tbkTitle.Text = $"{DateTimeOffset.Now:HH:mm:ss}  {msg}  {Path.GetFileNameWithoutExtension(changedFile)}  ";
    _ = lbxHist.Items.Add(tbkTitle.Text);

    Topmost = true;

    UseSayExe(msg); await Task.Delay(999);

    if (chkQuietMode.IsChecked == false)
    {
      if (changedFile.Contains(".Er▄▀."))
      {
        WindowState = WindowState.Normal;
        brdr1.Background = Brushes.Fuchsia;
        await Task.Run(async () => await StartAudioNotifier(PlayErrorFAF));
        await Task.Run(async () => await StartVisualNotifier());
      }
      else if (chkLongAudio.IsChecked == true && _ctsAudio is null) // do not "hide" error sound!!!
      {
        WindowState = WindowState.Normal;
        brdr1.Background = Brushes.Brown;
        await Task.Run(async () => await StartAudioNotifier(PlayQuietFAF));
      }
      else
      {
        PlayQuietFAF();
        brdr1.Background = Brushes.Yellow;
      }
    }

    Title = $"{DateTime.Now:HH:mm}  ▄▀▄▀▄▀▄▀   {VersionHelper.CurVerStrYMd}";

    _ = ReScanFolder_SetCurrentStateToWatchChangesAgainst(tbxPath.Text); // resets the mark to prevent the changes to be picked by the FS checker and re-start the alarm.
  }

  void PlayErrorFAF() => Task.Run(async () => await _bpr.WaveAsync(2000, 5000, 3));
  void PlayQuietFAF() => Task.Run(async () => await _bpr.WaveAsync(60, 401, 7)); //too quiet - worked on the old monitor speakers only: 060, 101, 7));

  async Task StartAudioNotifier(Action audio, [CallerMemberName] string? cmn = "")
  {
    WriteLine($"\n{DateTime.Now:HH:mm:ss}   Starting AUDIO by  {cmn} + + + + + + + + + + + + + + + ");
    _ctsAudio?.Cancel();
    _ctsAudio = new();
    _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => tbkHeadr.Text = $" {_w} {_v} {++_a} "));

    PeriodicTimer timer = new(TimeSpan.FromMilliseconds(1000));
    try
    {
      while (await timer.WaitForNextTickAsync(_ctsAudio.Token))
      {
        audio();
        _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => tbkHeadr.Text = $" {_i++}++ "));          //await Task.Delay(_i);
      }
    }
    catch (OperationCanceledException ex) { WriteLine("Cancelled AUDIO:  - - - - - - - - - - - - - - - - - - " + ex.Message); }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }
    finally
    {
      if (_ctsAudio is not null) { _ctsAudio.Dispose(); _ctsAudio = null; _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => tbkHeadr.Text = $" {_w} {_v} {--_a} ")); }
    }
  }
  async Task StartVisualNotifier([CallerMemberName] string? cmn = "")
  {
    WriteLine($"\n{DateTime.Now:HH:mm:ss}   Starting Visual by  {cmn} + + + + + + + + + + + + + + + ");
    _ctsVideo?.Cancel();
    _ctsVideo = new();
    _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => tbkHeadr.Text = $" {_w} {++_v} {_a} "));

    PeriodicTimer timer = new(TimeSpan.FromMilliseconds(_200ms + _200ms));
    try
    {
      while (await timer.WaitForNextTickAsync(_ctsVideo.Token))
      {
        Write($"v");

        _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(async () =>
        {
          Title = $"▄▀▄▀▄▀▄▀   {VersionHelper.CurVerStrYMd}"; await Task.Delay(_200ms);
          Title = $" ▄▀▄▀▄▀▄▀  {VersionHelper.CurVerStrYMd}"; await Task.Delay(_200ms);
          Title = $"  ▄▀▄▀▄▀▄▀ {VersionHelper.CurVerStrYMd}"; await Task.Delay(_200ms);
          Title = $"▀▄▀▄▀▄▀▄   {VersionHelper.CurVerStrYMd}"; await Task.Delay(_200ms);
          Title = $" ▀▄▀▄▀▄▀▄  {VersionHelper.CurVerStrYMd}"; await Task.Delay(_200ms);
          Title = $"  ▀▄▀▄▀▄▀▄ {VersionHelper.CurVerStrYMd}";
        }));
      }
    }
    catch (OperationCanceledException ex) { WriteLine("Cancelled Visual:  - - - - - - - - - - - - - - - - - - " + ex.Message); }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }
    finally
    {
      if (_ctsVideo is not null)
      {
        _ctsVideo.Dispose();
        _ctsVideo = null;
      }

      _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
      {
        Title = $"{DateTime.Now:HH:mm}  Out";
        tbkHeadr.Text = $" {_w} {--_v} {_a} ";
      }));
    }
  }
  async Task StartPeriodicChecker()
  {
    WriteLine($"\n{DateTime.Now:HH:mm:ss}   Starting Checkr   ");
    _ctsCheckr?.Cancel();
    _ctsCheckr = new();
    PeriodicTimer timer = new(TimeSpan.FromSeconds(60));
    try
    {
      while (await timer.WaitForNextTickAsync(_ctsCheckr.Token))
      {
        Write($"C");
        ChckFS();
      }
    }
    catch (OperationCanceledException ex) { WriteLine("Cancelled:  " + ex.Message); }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }
    finally { if (_ctsCheckr is not null) { _ctsCheckr.Dispose(); _ctsCheckr = null; } }
  }

  void OnWtchOn(object sender, RoutedEventArgs e) => StartWatch();
  async void OnWtchNo(object sender, RoutedEventArgs e) => await StopWatch();

  void OnSizeChanged(object sender, SizeChangedEventArgs e) => WriteLine($"{DateTime.Now:HH:mm:ss}  OnSizeChanged");

  async void OnStateChanged(object s, EventArgs e)
  {
    WriteLine($"{DateTime.Now:HH:mm:ss}  OnStateChanged({WindowState})");
    switch (WindowState)
    {
      default: break;
      case WindowState.Minimized: await AckAck(s); break;
    }
  }

  async void OnStart6(object sender, RoutedEventArgs e) => await StartVisualNotifier();
  void OnStop_6(object sender, RoutedEventArgs e)
  {
    WriteLine($"\nCancelling  ({DateTime.Now:HH:mm:ss})");
    PlayQuietFAF();
    try
    {
      _ctsVideo?.Cancel();
      _ctsAudio?.Cancel();
      WriteLine($"Cancelled   both !!!!! ({DateTime.Now:HH:mm:ss})");
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }
  }
  static void UseSayExe(string msg) => new Process { StartInfo = new ProcessStartInfo(@"say.exe", $"\"{msg}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
  void RemoveDeleteds()
  {
    do
    {
      foreach (var deletedFile in _userSettingsAndStateOfFS.FileDataList.Where(r => r.IsDeleted))
      {
        _ = _userSettingsAndStateOfFS.FileDataList.Remove(deletedFile);
        break;
      }
    } while (_userSettingsAndStateOfFS.FileDataList.Any(r => r.IsDeleted));
  }

  protected override async void OnClosed(EventArgs e)
  {
    await rsk.OnClosed(e);
    base.OnClosed(e);
  }
}