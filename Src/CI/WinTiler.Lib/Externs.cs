using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WinTiler.Lib
{
  public class Externs
  {
    public static void CloseWindow(IntPtr hwnd) => SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    public static void SetWindowPos(IntPtr hWnd, int x, int y, int width, int height) => SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0);
    public static bool EnumDesktopWindows(EnumDelegate filterCallback) => EnumDesktopWindows(IntPtr.Zero, filterCallback, IntPtr.Zero);
    public static bool IsVisible(IntPtr hWnd) => IsWindowVisible(hWnd);
    internal static uint GetWindowThreadProcessId_(IntPtr hWnd, out uint processID) => GetWindowThreadProcessId(hWnd, out processID);
    internal static int GetWindowText(IntPtr hWnd, StringBuilder winText) => GetWindowText(hWnd, winText, winText.Capacity);
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

    public static void Maximize(IntPtr handle) { ShowWindow(handle, ShowWindowCommands.Maximize); ; }
    public static void Minimize(IntPtr handle) { ShowWindow(handle, ShowWindowCommands.ShowMinimized); ; }
    public static void Restored(IntPtr handle) { ShowWindow(handle, ShowWindowCommands.ShowDefault); ; }

    public static WindowState GetPlacement(IntPtr hwnd)
    {
      var placement = new WINDOWPLACEMENT();
      placement.Length = Marshal.SizeOf(placement);
      GetWindowPlacement(hwnd, out placement);

      Debug.WriteLine($"          {placement.ShowCmd,-26} ");

      switch (placement.ShowCmd)
      {
        case ShowWindowCommands.Maximize:             /**/ return WindowState.Maximized;
        case ShowWindowCommands.Normal:               /**/ return WindowState.Normal;
        case ShowWindowCommands.ShowMinimized:        /**/ return WindowState.Minimized;
        case ShowWindowCommands.Hide:                 /**/ return WindowState.Minimized;
        case ShowWindowCommands.ShowNoActivate:       /**/ return WindowState.Minimized;
        case ShowWindowCommands.Show:                 /**/ return WindowState.Minimized;
        case ShowWindowCommands.Minimize:             /**/ return WindowState.Minimized;
        case ShowWindowCommands.ShowMinNoActive:      /**/ return WindowState.Minimized;
        case ShowWindowCommands.ShowNA:               /**/ return WindowState.Minimized;
        case ShowWindowCommands.Restore:              /**/ return WindowState.Minimized;
        case ShowWindowCommands.ShowDefault:          /**/ return WindowState.Minimized;
        case ShowWindowCommands.ForceMinimize:        /**/ return WindowState.Minimized;
        default:                                      /**/ return WindowState.Minimized;
      }
    }


    public delegate bool EnumDelegate(IntPtr hWnd, int lParam);


    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)] static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);
    [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)] static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);
    [DllImport("user32.dll", SetLastError = true)] static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("kernel32.dll", SetLastError = true)] static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);
    [DllImport("psapi.dll")] static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);    /// Retrieves the fully-qualified path for the file containing the specified module.    /// http://msdn.microsoft.com/en-us/library/ms683198(VS.85).aspx
    [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll", CharSet = CharSet.Auto)] static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
    [DllImport("user32.dll", SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);
    [DllImport("user32.dll", SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
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

    [Flags()]
    public enum SetWindowPosFlags : uint    // Define the SetWindowPosFlags enumeration.
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

    const uint WM_CLOSE = 0x0010;
  }
}
