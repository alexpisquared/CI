using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WinMgr
{
  public class MinMaxRestorer
  {
    public static void CloseWindow(string processName)
    {
      var processes = Process.GetProcessesByName(processName);
      if (processes.Length > 0)
      {
        foreach (var process in processes)
        {
          var windows = List_Windows_By_PID(process.Id);
          foreach (var pair in windows)
          {
            var placement = new WINDOWPLACEMENT();
            GetWindowPlacement(pair.Key, ref placement);
            ShowWindowAsync(pair.Key, SW_SHOWNORMAL);
            //ShowWindowAsync(pair.Key, placement.showCmd == SW_SHOWMINIMIZED ? SW_SHOWMAXIMIZED : SW_SHOWMINIMIZED);
          }
        }
      }
    }
    public static IDictionary<IntPtr, string> List_Windows_By_PID(int processID)
    {
      var hShellWindow = GetShellWindow();
      var dictWindows = new Dictionary<IntPtr, string>();

      EnumWindows(delegate (IntPtr hWnd, int lParam)
      {
        //ignore the shell window
        if (hWnd == hShellWindow)
        {
          return true;
        }

        //ignore non-visible windows
        if (!IsWindowVisible(hWnd))
        {
          return true;
        }

        //ignore windows with no text
        var length = GetWindowTextLength(hWnd);
        if (length == 0)
        {
          return true;
        }

        GetWindowThreadProcessId(hWnd, out var windowPid);

        //ignore windows from a different process
        if (windowPid != processID)
        {
          return true;
        }

        var stringBuilder = new StringBuilder(length);
        GetWindowText(hWnd, stringBuilder, length + 1);
        dictWindows.Add(hWnd, stringBuilder.ToString());

        return true;

      }, 0);

      return dictWindows;
    }


    const int SW_SHOWNORMAL = 1;
    const int SW_SHOWMINIMIZED = 2;
    const int SW_SHOWMAXIMIZED = 3;

    struct WINDOWPLACEMENT
    {
      public int length;
      public int flags;
      public int showCmd;
      public System.Drawing.Point ptMinPosition;
      public System.Drawing.Point ptMaxPosition;
      public System.Drawing.Rectangle rcNormalPosition;
    }

    delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    [DllImport("user32.dll")] static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll", SetLastError = true)] static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("USER32.DLL")] static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
    [DllImport("USER32.DLL")] static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    [DllImport("USER32.DLL")] static extern int GetWindowTextLength(IntPtr hWnd);
    [DllImport("USER32.DLL")] static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("USER32.DLL")] static extern IntPtr GetShellWindow();
    [DllImport("USER32.DLL")] static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

  }
}
