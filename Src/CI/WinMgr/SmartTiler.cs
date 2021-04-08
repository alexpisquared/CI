using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Console = Colorful.Console;

/// http://csharphelper.com/blog/2016/12/tile-desktop-windows-in-rows-and-columns-in-c/
/// also for Virt Desktop see:
///   https://github.com/MScholtes/VirtualDesktop

namespace WinMgr
{
  public class SmartTiler
  {
    readonly List<WindowInfo> _allWindows = new();
    readonly VDM _vdm = new();

    //public SmartTiler() => collectDesktopWindows();

    public void Arrange()
    {
      while (true)
      {
        Console.Clear();
        Console.BackgroundColor = Color.FromArgb(32,16,0);
        collectDesktopWindows();
        if (_allWindows.Count < 1)
        {
          Console.WriteLine($"--- No valid windows found ---");
          return;
        }

        var screen = Screen.PrimaryScreen;      //foreach (var screen in WindowsFormsLib.WinFormHelper.GetAllScreens()) Console.WriteLine($"{screen}");
        int cols = 3, rows = 3, rp1 = 1;

        if (_allWindows.Count < 4) { cols = 2; rows = 1; }
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
          if (++c >= cols)
          {
            c = 0;
            x = screen.WorkingArea.Left;
            y += window_height;
            var cl = _allWindows.Count - i;
            if (cl < cols && cl != 0)
            {
              window_width = screen.WorkingArea.Width / cl;
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

    struct WindowInfo
    {
      public string WTitle;
      public string Sorter;
      public IntPtr Handle;

      public WindowInfo(string title, IntPtr handle)
      {
        WTitle = title;
        Handle = handle;
        var appName = title.Split(" - ");
        Sorter = appName.Length > 1 ? $"{appName.LastOrDefault()} · {appName.FirstOrDefault()}" : $"· {WTitle}";
      }

      public override string ToString() => Sorter;
    }
  }


  static class DesktopWindowsStuff
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

    static VDM _vdm;

    public static void GetDesktopWindowHandlesAndTitles(out List<IntPtr> handles, out List<string> titles, VDM vdm)
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
        && !title.Contains("Microsoft Visual Studio")
        && !title.Contains("Windows Shell Experience Host")
        && !title.Contains("Settings")
        && !title.Contains("GitHub")
        && !title.Contains("Remote Desktop Connection")
        && !title.Contains("Team")
        && !title.Contains("Outlook")
        && !title.Contains("Setup")
        && !title.Contains("Program Manager")
        && !title.Contains("Microsoft Text Input Application")
        && !title.Contains("DiReq")   // scrsvr
        && !title.Contains("WinMgr")  // us
        && _vdm.IsWindowOnCurrentVirtualDesktop(hWnd)
        )
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
