using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WinTiler.Lib
{
  public static class DesktopWindowsStuff
  {    
    static List<IntPtr> WindowHandles;
    static List<string> WindowTitles;
    static List<string> ExePaths;
    static VirtDesktopMgr _vdm;

    public static void GetDesktopWindowHandlesAndTitles(out List<IntPtr> handles, out List<string> titles, out List<string> epaths, VirtDesktopMgr vdm)
    {
      WindowHandles = new List<IntPtr>();
      WindowTitles = new List<string>();
      ExePaths = new List<string>();
      _vdm = vdm;

      if (!Externs.EnumDesktopWindows( filterCallback))
      {
        handles = null;
        titles = null;
        epaths = null;
      }
      else
      {
        handles = WindowHandles;
        titles = WindowTitles;
        epaths = ExePaths;
      }
    }

    static bool filterCallback(IntPtr hWnd, int lParam)
    {
      var winText = new StringBuilder(1024);
      _ = Externs.GetWindowText(hWnd, winText);
      var title = winText.ToString();

      _ = Externs.GetWindowThreadProcessId_(hWnd, out var processID);

      var p = Process.GetProcessById((int)processID);
      var exePath = p.ProcessName;
      //exePath =      p.MainModule.ModuleName;

      if (Externs.IsVisible(hWnd) && string.IsNullOrEmpty(title) == false
        && !title.Contains("Calculator")
        && !title.Contains("DiReq")   // scrsvr
        && !title.Contains("GitHub")
        && !title.Contains("Microsoft Store")
        && !title.Contains("Microsoft Text Input Application")
        && !title.Contains("Microsoft Visual Studio")
        && !title.Contains("Outlook")
        && !title.Contains("Program Manager")
        && !title.Contains("Remote Desktop Connection")
        && !title.Contains("Settings")
        && !title.Contains("Setup")
        && !title.Contains("Task Manager") // un movable/sizeable
        && !title.Contains("Team")
        && !title.Contains("Windows Shell Experience Host")
        && !title.Contains("Window Tiler")  // us
        && _vdm.IsWindowOnCurrentVirtualDesktop(hWnd))
      {
        WindowHandles.Add(hWnd);
        WindowTitles.Add(title);
        ExePaths.Add(exePath);
      }
      //else
      //  Console.Write($" {title} ");

      return true; // Return true to indicate that we should continue enumerating windows.
    }
  }
}