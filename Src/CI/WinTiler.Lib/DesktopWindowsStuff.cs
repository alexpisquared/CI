using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinTiler.Lib
{
  public static class DesktopWindowsStuff
  {
    #region "Find Desktop Windows"
    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)] static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);
    [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)] static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

    // Define the SetWindowPosFlags enumeration.
    [Flags()]
    public enum SetWindowPosFlags : uint
    {
      SynchronousWindowPosition = 0x4000,
      DeferErase = 0x2000,
      DrawFrame = 0x0020,
      FrameChanged = 0x0020,
      HideWindow = 0x0080,
      DoNotActivate = 0x0010,
      DoNotCopyBits = 0x0100,
      IgnoreMove = 0x0002,
      DoNotChangeOwnerZOrder = 0x0200,
      DoNotRedraw = 0x0008,
      DoNotReposition = 0x0200,
      DoNotSendChangingEvent = 0x0400,
      IgnoreResize = 0x0001,
      IgnoreZOrder = 0x0004,
      ShowWindow = 0x0040,
    }

    // Define the callback delegate's type.
    delegate bool EnumDelegate(IntPtr hWnd, int lParam);

    // Save window titles and handles in these lists.
    static List<IntPtr> WindowHandles;
    static List<string> WindowTitles;

    static VirtDesktopMgr _vdm;

    public static void GetDesktopWindowHandlesAndTitles(out List<IntPtr> handles, out List<string> titles, VirtDesktopMgr vdm)
    {
      WindowHandles = new List<IntPtr>();
      WindowTitles = new List<string>();
      _vdm = vdm;

      if (!EnumDesktopWindows(IntPtr.Zero, FilterCallback, IntPtr.Zero))
      {
        handles = null;
        titles = null;
      }
      else
      {
        handles = WindowHandles;
        titles = WindowTitles;
      }
    }

    // We use this function to filter windows.
    // This version selects visible windows that have titles.
    static bool FilterCallback(IntPtr hWnd, int lParam)
    {
      var sb = new StringBuilder(1024);
      _ = GetWindowText(hWnd, sb, sb.Capacity);
      var title = sb.ToString();

      if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(title) == false
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
        && !title.Contains("WinMgr")  // us
        && _vdm.IsWindowOnCurrentVirtualDesktop(hWnd))
      {
        WindowHandles.Add(hWnd);
        WindowTitles.Add(title);
        //Console.Write($"■ {title,-22}  \n");
      }
      //else
      //  Console.Write($" {title} ");

      return true; // Return true to indicate that we should continue enumerating windows.
    }
    #endregion "Find Desktop Windows"

    #region "SetWindowPos"
    // Define the SetWindowPos API function.
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    // Wrapper for SetWindowPos.
    public static void SetWindowPos(IntPtr hWnd, int x, int y, int width, int height) => SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0);
    #endregion "SetWindowPos"

    #region "SetWindowPlacement"
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct WINDOWPLACEMENT
    {
      public int Length;
      public int Flags;
      public ShowWindowCommands ShowCmd;
      public POINT MinPosition;
      public POINT MaxPosition;
      public RECT NormalPosition;
      public static WINDOWPLACEMENT Default
      {
        get
        {
          var result = new WINDOWPLACEMENT();
          result.Length = Marshal.SizeOf(result);
          return result;
        }
      }
    }

    public enum ShowWindowCommands : int
    {
      Hide = 0,
      Normal = 1,
      ShowMinimized = 2,
      Maximize = 3, // is this the right value?
      ShowMaximized = 3,
      ShowNoActivate = 4,
      Show = 5,
      Minimize = 6,
      ShowMinNoActive = 7,
      ShowNA = 8,
      Restore = 9,
      ShowDefault = 10,
      ForceMinimize = 11
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public int X;
      public int Y;

      public POINT(int x, int y)
      {
        X = x;
        Y = y;
      }

      public static implicit operator System.Drawing.Point(POINT p) => new System.Drawing.Point(p.X, p.Y);

      public static implicit operator POINT(System.Drawing.Point p) => new POINT(p.X, p.Y);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      readonly int _Left;
      readonly int _Top;
      readonly int _Right;
      readonly int _Bottom;
    }

    // Wrapper for SetWindowPlacement.
    public static void SetWindowPlacement(IntPtr handle, ShowWindowCommands show_command)
    {
      // Prepare the WINDOWPLACEMENT structure.
      var placement = new WINDOWPLACEMENT();
      placement.Length = Marshal.SizeOf(placement);

      // Get the window's current placement.
      GetWindowPlacement(handle, out placement);

      // Perform the action.
      placement.ShowCmd = show_command;
      SetWindowPlacement(handle, ref placement);
    }
    #endregion "SetWindowPlacement"

  }
}