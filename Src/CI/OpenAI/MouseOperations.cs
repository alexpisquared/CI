﻿using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenAI;
public class MouseOperations
{
  [DllImport("user32.dll", EntryPoint = "SetCursorPos")][return: MarshalAs(UnmanagedType.Bool)]  static extern bool SetCursorPos(int x, int y);
  [DllImport("user32.dll")][return: MarshalAs(UnmanagedType.Bool)] static extern bool GetCursorPos(out MousePoint lpMousePoint);
  [DllImport("user32.dll")] static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

  public static void SetCursorPosition(int x, int y) => SetCursorPos(x, y);
  public static void SetCursorPosition(MousePoint point) => SetCursorPos(point.X, point.Y);
  public static MousePoint GetCursorPosition()
  {
    var gotPoint = GetCursorPos(out var currentMousePoint);
    if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }

    return currentMousePoint;
  }
  public static void MouseClickEvent(int x, int y)
  {

    _ = SetCursorPos(x, y); // without actually moving cursor does not seem to be clicking on the indicated spot.

    if (SystemInformation.MouseButtonsSwapped)
      MouseEvent(x, y, MouseEventFlags.RightDown | MouseEventFlags.RightUp);
    else
      MouseEvent(x, y, MouseEventFlags.LeftDown | MouseEventFlags.LeftUp);
  }
  public static void MouseEvent(MouseEventFlags value)
  {
    var position = GetCursorPosition();

    mouse_event((int)value, position.X, position.Y, 0, 0);
  }
  public static void MouseEvent(int x, int y, MouseEventFlags value) => mouse_event((int)value, x, y, 0, 0);

  [StructLayout(LayoutKind.Sequential)]
  public struct MousePoint
  {
    public int X;
    public int Y;

    public MousePoint(int x, int y)
    {
      X = x;
      Y = y;
    }
  }

  [Flags]
  public enum MouseEventFlags
  {
    LeftDown = 0x00000002,
    LeftUp = 0x00000004,
    MiddleDown = 0x00000020,
    MiddleUp = 0x00000040,
    Move = 0x00000001,
    Absolute = 0x00008000,
    RightDown = 0x00000008,
    RightUp = 0x00000010
  }
}
