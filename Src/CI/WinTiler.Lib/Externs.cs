using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinTiler.Lib
{
  class Externs
  {
    [DllImport("user32.dll", CharSet = CharSet.Auto)]private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    private const UInt32 WM_CLOSE = 0x0010;

    public static void CloseWindow(IntPtr hwnd)
    {
      SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    }
  }
}
