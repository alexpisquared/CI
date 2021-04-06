using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

/// http://csharphelper.com/blog/2016/12/tile-desktop-windows-in-rows-and-columns-in-c/
/// also for Virt Desktop see:
///   https://www.codeproject.com/Articles/7666/Desktop-Switching
///   https://www.cyberforum.ru/blogs/105416/blog3671.html
///   https://docs.microsoft.com/en-us/archive/blogs/winsdk/virtual-desktop-switching-in-windows-10
///   https://github.com/MScholtes/VirtualDesktop

namespace WinMgr
{
  public class SmartTiler
  {
    readonly List<WindowInfo> _allWindows = new();
    int _cols = 3, _rows = 3;

    public SmartTiler() => CollectDesktopWindows();

    struct WindowInfo
    {
      public string Title;
      public IntPtr Handle;

      public WindowInfo(string title, IntPtr handle)
      {
        Title = title;
        Handle = handle;
      }

      public override string ToString() => Title;
    }

    void CollectDesktopWindows()
    {
      DesktopWindowsStuff.GetDesktopWindowHandlesAndTitles(out var handles, out var titles);

      Console.WriteLine($"{titles.Count(),3}  windows with titles total:");

      _allWindows.Clear();
      for (var i = 0; i < titles.Count; i++)
      {
        Console.WriteLine($"{i,3}  {titles[i],-22}  ");
        _allWindows.Add(new WindowInfo(titles[i], handles[i]));
      }
    }

    public void btnArrange_Click()
    {
      foreach (var screen in WindowsFormsLib.WinFormHelper.GetAllScreens()) Console.WriteLine(screen);

      var screen_top = Screen.PrimaryScreen.WorkingArea.Top;
      var screen_left = Screen.PrimaryScreen.WorkingArea.Left;
      var screen_width = Screen.PrimaryScreen.WorkingArea.Width;
      var screen_height = Screen.PrimaryScreen.WorkingArea.Height;

      var rr = (int)(Math.Sqrt(_allWindows.Count) - .5);
      _rows = _cols = rr;

      var window_width = screen_width / _cols - 20;
      var window_height = screen_height / _rows - 20;

      var window_num = 0;
      var y = screen_top;
      for (var row = 0; row < _rows; row++)
      {
        var x = screen_left;
        for (var col = 0; col < _cols; col++)
        {
          DesktopWindowsStuff.SetWindowPlacement(_allWindows[window_num].Handle, DesktopWindowsStuff.ShowWindowCommands.Restore);
          DesktopWindowsStuff.SetWindowPos(_allWindows[window_num].Handle, x, y, window_width, window_height);

          if (++window_num >= _allWindows.Count) return;
          x += window_width;
        }
        y += window_height;
      }
    }
  }


  static class DesktopWindowsStuff
  {
    #region "Find Desktop Windows"
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", EntryPoint = "GetWindowText",
    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

    [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool EnumDesktopWindows(IntPtr hDesktop,
        EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

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

    // Return a list of the desktop windows' handles and titles.
    public static void GetDesktopWindowHandlesAndTitles(out List<IntPtr> handles, out List<string> titles)
    {
      WindowHandles = new List<IntPtr>();
      WindowTitles = new List<string>();

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
      // Get the window's title.
      var sb_title = new StringBuilder(1024);
      var length = GetWindowText(hWnd, sb_title, sb_title.Capacity);
      var title = sb_title.ToString();


      if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(title) == false && !title.Contains("Microsoft Visual Studio"))
      {
        WindowHandles.Add(hWnd);
        WindowTitles.Add(title);
        Console.Write($"\n■ {title,-22}  ");
      }
      else
        Console.Write($" {title} ");

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
    static extern bool SetWindowPlacement(IntPtr hWnd,
       [In] ref WINDOWPLACEMENT lpwndpl);

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

    internal enum ShowWindowCommands : int
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
