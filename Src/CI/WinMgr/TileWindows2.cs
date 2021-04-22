//using System;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Threading;

//namespace WinMgr
//{
//  public class TileWindows2
//  {
//    internal struct RECT
//    {
//      public int left;
//      public int top;
//      public int right;
//      public int bottom;
//    }

//    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
//    internal static extern IntPtr GetForegroundWindow();

//    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
//    internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

//    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
//    internal static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
//    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
//    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);


//    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)] static extern unsafe bool CascadeWindows(IntPtr hWnd, int wHow, RECT[] lpRect, uint cKids, IntPtr[] lpKids);
//    [DllImport("user32.dll", EntryPoint = "TileWindows")] static extern unsafe IntPtr TileWindows(IntPtr hWnd, int wHow, RECT[] lpRect, uint cKids, IntPtr[] lpKids);
//    [DllImport("User32.dll")] static extern int FindWindow(string lpClassName, string lpWindowName);

//    public static void Tile()
//    {
//      const short SWP_NOZORDER = 0X4;
//      const int SWP_SHOWWINDOW = 0x0040;
//      const int MDITILE_VERTICAL = 0x0000;
//      const short SWP_ASYNCWINDOWPOS = 0x4000;
//      try
//      {
//        var outlook = new Process();
//        outlook.StartInfo.FileName = "notepad.exe";
//        outlook.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//        outlook.Start();
//        var id = outlook.MainWindowHandle;
//        if (id != IntPtr.Zero)
//        {
//          id = GetForegroundWindow();
//          SetWindowPos(id, 0, 0, 0, 800, 900, SWP_NOZORDER | SWP_SHOWWINDOW | SWP_ASYNCWINDOWPOS);
//        }

//        Thread.Sleep(2000);
//        var explorer = new Process();
//        explorer.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//        explorer.StartInfo.FileName = "explorer.exe";
//        explorer.Start();

//        var id2 = explorer.MainWindowHandle;

//        if (id2 != IntPtr.Zero)
//        {
//          id2 = GetForegroundWindow();
//          SetWindowPos(id2, 0, -800, 0, 800, 900, SWP_NOZORDER | SWP_SHOWWINDOW);
//        }

//        TileWindows(id2, MDITILE_VERTICAL, null, 0, null);
//      }
//      catch (Exception ex) { Console.WriteLine(ex); }
//    }
//  }
//}
