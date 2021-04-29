using System;
using System.Runtime.InteropServices;
//todo: using WindowsFormsLib;
/// <summary>
/// also 
///  Move the window of another process to any Virtual Desktop (Support in version 2.0 or later) ???is it current? . https://github.com/Grabacr07/VirtualDesktop
/// 
/// 
/// https://github.com/MScholtes/VirtualDesktop  ++++++++++++++++++!!!!!!!!!!!!!???????????
/// MoveWindow:<s|n>        move process with name <s> or id <n> to desktop with number in pipeline (short: /mw).
/// MoveWindowHandle:<s|n>  move window with text <s> in title or handle <n> to desktop with number in pipeline (short: / mwh).
/// </summary>

namespace WinTiler.Lib
{
  public class VirtDesktopMgr
  {
    readonly VirtualDesktopManager _vdm = new VirtualDesktopManager();

    //todo: void PopupDetails(IntPtr _handle) => MessageBox.Show("Virtual Desktop ID: " + _vdm.GetWindowDesktopId(_handle).ToString("X") + Environment.NewLine + "IsCurrentVirtualDesktop: " + _vdm.IsWindowOnCurrentVirtualDesktop(_handle).ToString());
    public bool IsWindowOnCurrentVirtualDesktop(IntPtr _handle) => _vdm.IsWindowOnCurrentVirtualDesktop(_handle);
    public void MoveToCurrentDesktop(IntPtr _handle)
    {
      try
      {
        if (!_vdm.IsWindowOnCurrentVirtualDesktop(_handle))
        {
          //todo:       using var nw = new EmptyFormWindow();
          //todo:       nw.Show(null);
          //todo:       _vdm.MoveWindowToDesktop(_handle, _vdm.GetWindowDesktopId(nw.Handle));
        }
      }
      catch (Exception ex) { Console.WriteLine($" ■■■ {ex.Message}"); }
    }
  }

  [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
  [System.Security.SuppressUnmanagedCodeSecurity]
  public interface IVirtualDesktopManager
  {
    [PreserveSig] int IsWindowOnCurrentVirtualDesktop([In] IntPtr TopLevelWindow, [Out] out int OnCurrentDesktop);
    [PreserveSig] int GetWindowDesktopId([In] IntPtr TopLevelWindow, [Out] out Guid CurrentDesktop);
    [PreserveSig] int MoveWindowToDesktop([In] IntPtr TopLevelWindow, [MarshalAs(UnmanagedType.LPStruct)][In] Guid CurrentDesktop);
  }



  [ComImport, Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")] public class CVirtualDesktopManager { }

  public class VirtualDesktopManager
  {
    CVirtualDesktopManager? cmanager = null;
    IVirtualDesktopManager? manager;

    public VirtualDesktopManager()
    {
      cmanager = new CVirtualDesktopManager();
      manager = (IVirtualDesktopManager)cmanager;
    }
    ~VirtualDesktopManager()
    {
      manager = null;
      cmanager = null;
    }
    public bool IsWindowOnCurrentVirtualDesktop(IntPtr TopLevelWindow)
    {
      int hr;
      if ((hr = manager.IsWindowOnCurrentVirtualDesktop(TopLevelWindow, out var result)) != 0)
      {
        Marshal.ThrowExceptionForHR(hr);
      }
      return result != 0;
    }
    public Guid GetWindowDesktopId(IntPtr TopLevelWindow)
    {
      int hr;
      if ((hr = manager.GetWindowDesktopId(TopLevelWindow, out var result)) != 0)
      {
        Marshal.ThrowExceptionForHR(hr);
      }
      return result;
    }
    public void MoveWindowToDesktop(IntPtr TopLevelWindow, Guid CurrentDesktop)
    {
      int hr;
      if ((hr = manager?.MoveWindowToDesktop(TopLevelWindow, CurrentDesktop) ?? 0) != 0)
      {
        Marshal.ThrowExceptionForHR(hr);
      }
    }
  }
}
