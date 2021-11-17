using CI.Standard.Lib.Helpers;
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
    readonly UserPrefs _userPrefs = JsonIsoFileSerializer.Load<UserPrefs>() ?? new UserPrefs();
    string _report = "";

    public SmartTiler()
    {
      Debug.WriteLine($"** {_userPrefs.ExesToIgnore.Count} app names loaded:  {string.Join(' ', _userPrefs.ExesToIgnore)}");
      Debug.WriteLine($"** {_userPrefs.TitlToIgnore.Count} wi titles loaded:  {string.Join(' ', _userPrefs.TitlToIgnore)}");
    }

    public ObservableCollection<WindowInfo> AllWindows => _allWindows;

    public bool SkipMinimized { get => _userPrefs.SkipMinimized; set => _userPrefs.SkipMinimized = value; }
    public string Report { get => _report; }

    public string CollectDesktopWindows(bool? sm = null)
    {
      if (sm is not null)
        SkipMinimized = sm.Value;

      var sw = Stopwatch.StartNew();
      DesktopWindowsStuff.GetDesktopWindowHandlesAndTitles(out var handles, out var titles, out var epaths, _vdm, _userPrefs, _userPrefs.SkipMinimized);

      var lst = new List<WindowInfo>();
      _allWindows.Clear(); for (var i = 0; i < titles?.Count; i++) lst.Add(new WindowInfo(titles[i], epaths?[i] ?? "..epaths is null", handles?[i] ?? IntPtr.Zero));

      lst.OrderBy(r => r.Sorter).ToList().ForEach(r => _allWindows.Add(new WindowInfo(r.WTitle, r.AppNme, r.Handle)));

      return (_report = $" ... Found {titles?.Count} windows of interest in {sw.Elapsed.TotalSeconds:N1} s: ");
    }
    public void Tile()
    {
      var screen = WindowsFormsLib.WinFormHelper.PrimaryScreen;      //foreach (var screen in WindowsFormsLib.WinFormHelper.GetAllScreens()) Console.WriteLine($"{screen}");
      int cols = 3, rows = 3, rp1 = 1;

      if (_allWindows.Count <= 2) { cols = 2; rows = 1; }
      else if (_allWindows.Count <= 05) { cols = _allWindows.Count; rows = 1; }
      else if (_allWindows.Count <= 08) { cols = 4; rows = 2; }
      else if (_allWindows.Count <= 12) { cols = 4; rows = 3; }
      else if (_allWindows.Count <= 16) { cols = 4; rows = 4; }
      else if (_allWindows.Count <= 20) { cols = 5; rows = 4; }
      else if (_allWindows.Count <= 25) { cols = 5; rows = 5; }
      else if (_allWindows.Count <= 30) { cols = 5; rows = 6; }
      else if (_allWindows.Count <= 36) { cols = 6; rows = 6; }
      else if (_allWindows.Count <= 64) { cols = 8; rows = 8; }
      else { rp1 = 1 + (int)Math.Sqrt(_allWindows.Count); rows = cols = rp1; }

      var window_width = screen.WorkingArea.Width / cols;
      var window_height = screen.WorkingArea.Height / rows;

      Console.WriteLine($"\n== {_allWindows.Count}  ->  {rp1}  ->  {rows} x {cols}  ->  {window_width} x {window_height}\n", Color.Cyan);

      var y = screen.WorkingArea.Top;
      var x = screen.WorkingArea.Left;
      int c = 0, i = 0;
      const int marg = 10;
      const int extn = marg + marg;
      foreach (var w in _allWindows.OrderBy(r => r.Sorter))
      {
        Externs.SetWindowPlacement(w.Handle, Externs.ShowWindowCommands.Restore);
        Externs.SetWindowPos(w.Handle, x - marg, y - marg, window_width + extn, window_height + extn);
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
    public void AddToIgnoreByExeName(string exePth) { _userPrefs.ExesToIgnore.Add(exePth); JsonIsoFileSerializer.Save(_userPrefs); }
    public void AddToIgnoreByWiTitle(string wTitle) { _userPrefs.TitlToIgnore.Add(wTitle); JsonIsoFileSerializer.Save(_userPrefs); }

    void closeAll(string v)
    {
      foreach (var w in _allWindows.Where(r => r.WTitle.Contains(v)))
      {
        Externs.CloseWindow(w.Handle);
      }
    }
  }
}
