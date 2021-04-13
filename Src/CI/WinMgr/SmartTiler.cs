using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinTiler.Lib;
using Console = Colorful.Console;

/// http://csharphelper.com/blog/2016/12/tile-desktop-windows-in-rows-and-columns-in-c/
/// also for Virt Desktop see:
///   https://github.com/MScholtes/VirtualDesktop

namespace WinMgr
{
  public class SmartTiler
  {
    readonly List<WindowInfo> _allWindows = new();
    readonly VirtDesktopMgr _vdm = new();

    //public SmartTiler() => collectDesktopWindows();

    public void Arrange()
    {
      while (true)
      {
        Console.Clear();
        Console.BackgroundColor = Color.FromArgb(32, 16, 0);
        collectDesktopWindows();
        if (_allWindows.Count < 1)
        {
          Console.WriteLine($"--- No valid windows found ---");
          return;
        }

        var screen = Screen.PrimaryScreen;      //foreach (var screen in WindowsFormsLib.WinFormHelper.GetAllScreens()) Console.WriteLine($"{screen}");
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
          DesktopWindowsStuff.SetWindowPlacement(w.Handle, DesktopWindowsStuff.ShowWindowCommands.Restore);
          DesktopWindowsStuff.SetWindowPos(w.Handle, x, y, window_width, window_height);
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

        Console.WriteLine($"Enter\tRedo\n" +
          $"E\tClose all Explorers  'This PC'\n" +
          $"\n" +
          $"\n" +
          $"", Color.Cyan);


        switch (Console.ReadKey(true).Key)
        {
          case ConsoleKey.Enter: break;
          case ConsoleKey.E: closeAll("This PC"); break;
          default: return;
        }
      }
    }

    void closeAll(string v)
    {
      foreach (var w in _allWindows.Where(r => r.WTitle.Contains(v)))
      {
        Externs.CloseWindow(w.Handle);
      }
    }
    void collectDesktopWindows()
    {
      DesktopWindowsStuff.GetDesktopWindowHandlesAndTitles(out var handles, out var titles, _vdm);

      Console.WriteLine($" ... Found  {titles.Count}  Windows of interest: ", Color.Gray);

      _allWindows.Clear();
      for (var i = 0; i < titles.Count; i++) _allWindows.Add(new WindowInfo(titles[i], handles[i]));

      var c = 0;
      foreach (var w in _allWindows.OrderBy(r => r.Sorter)) Console.WriteLine($"{++c,4}  {w}  ");
    }
  }
}
