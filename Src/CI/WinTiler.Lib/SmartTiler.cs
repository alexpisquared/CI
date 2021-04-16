using CI.GUI.Support.WpfLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

/// http://csharphelper.com/blog/2016/12/tile-desktop-windows-in-rows-and-columns-in-c/
/// also for Virt Desktop see:
///   https://github.com/MScholtes/VirtualDesktop

namespace WinTiler.Lib
{
  public class SmartTiler
  {
    readonly ObservableCollection<WindowInfo> _allWindows = new();
    readonly VirtDesktopMgr _vdm = new();
    readonly UserPrefs _up = JsonIsoFileSerializer.Load<UserPrefs>();
    string _report;

    public SmartTiler()
    {
      Debug.WriteLine($"** {_up.ExesToIgnore.Count} app names loaded:  {string.Join(' ', _up.ExesToIgnore)}");
      Debug.WriteLine($"** {_up.TitlToIgnore.Count} wi titles loaded:  {string.Join(' ', _up.TitlToIgnore)}");
    }

    public ObservableCollection<WindowInfo> AllWindows => _allWindows;

    public bool SkipMinimized { get => _up.SkipMinimized; set => _up.SkipMinimized = value; }
    public string Report { get => _report; }

    public string CollectDesktopWindows(bool? sm = null)
    {
      if (sm is not null)
        SkipMinimized = sm.Value;

      var sw = Stopwatch.StartNew();
      DesktopWindowsStuff.GetDesktopWindowHandlesAndTitles(out var handles, out var titles, out var epaths, _vdm, _up, _up.SkipMinimized);

      var lst = new List<WindowInfo>();
      _allWindows.Clear(); for (var i = 0; i < titles.Count; i++) lst.Add(new WindowInfo(titles[i], epaths[i], handles[i]));

      lst.OrderBy(r => r.Sorter).ToList().ForEach(r =>        _allWindows.Add(new WindowInfo(r.WTitle, r.AppNme, r.Handle)));

      return (_report = $" ... Found {titles.Count} windows of interest in {sw.Elapsed.TotalSeconds:N1} s: ");
    }
    public void Tile()
    {
      var screen = WindowsFormsLib.WinFormHelper.PrimaryScreen;      //foreach (var screen in WindowsFormsLib.WinFormHelper.GetAllScreens()) Console.WriteLine($"{screen}");
      int cols = 3, rows = 3, rp1 = 1;

      if (_allWindows.Count < 3) { cols = 2; rows = 1; }
      else if (_allWindows.Count < 4) { cols = 3; rows = 1; }
      else if (_allWindows.Count < 07) { cols = 3; rows = 2; }
      else if (_allWindows.Count < 10) { cols = 3; rows = 3; }
      else if (_allWindows.Count < 13) { cols = 3; rows = 4; }
      else if (_allWindows.Count < 17) { cols = 4; rows = 4; }
      else if (_allWindows.Count < 21) { cols = 4; rows = 5; }
      else if (_allWindows.Count < 26) { cols = 5; rows = 5; }
      else if (_allWindows.Count < 31) { cols = 5; rows = 6; }
      else if (_allWindows.Count < 36) { cols = 6; rows = 6; }
      else if (_allWindows.Count < 64) { cols = 8; rows = 8; }
      else { rp1 = 1 + (int)Math.Sqrt(_allWindows.Count); rows = cols = rp1; }

      var window_width = screen.WorkingArea.Width / cols;
      var window_height = screen.WorkingArea.Height / rows;

      Console.WriteLine($"\n== {_allWindows.Count}  ->  {rp1}  ->  {rows} x {cols}  ->  {window_width} x {window_height}\n", Color.Cyan);

      var y = screen.WorkingArea.Top;
      var x = screen.WorkingArea.Left;
      int c = 0, i = 0;
      foreach (var w in _allWindows.OrderBy(r => r.Sorter))
      {
        Externs.SetWindowPlacement(w.Handle, Externs.ShowWindowCommands.Restore);
        Externs.SetWindowPos(w.Handle, x, y, window_width, window_height);
        i++;
        x += window_width;
        if (++c >= cols) // ..to next row
        {
          c = 0;
          x = screen.WorkingArea.Left;
          y += window_height;

          var windowsLeft = _allWindows.Count - i;
          //if (windowsLeft == 1)
          //  window_width = screen.WorkingArea.Width - x;
          //else
          if (windowsLeft < cols && windowsLeft != 0)
          {
            window_width = screen.WorkingArea.Width / windowsLeft;
            window_height = screen.WorkingArea.Height - y;
          }
        }
      }
    }
    public void AddToIgnoreByExeName(string exePth) { _up.ExesToIgnore.Add(exePth); JsonIsoFileSerializer.Save(_up); }
    public void AddToIgnoreByWiTitle(string wTitle) { _up.TitlToIgnore.Add(wTitle); JsonIsoFileSerializer.Save(_up); }

    void closeAll(string v)
    {
      foreach (var w in _allWindows.Where(r => r.WTitle.Contains(v)))
      {
        Externs.CloseWindow(w.Handle);
      }
    }
  }
}
