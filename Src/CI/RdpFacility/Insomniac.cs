using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RdpFacility
{
  internal class Insomniac
  {
    internal void RequestActive()
    {
      SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
      Debug.WriteLine($"** On  since {DateTime.Now:HH:mm}");
    }
    internal void RequestRelease()
    {
      SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
      Debug.WriteLine($"** Off since {DateTime.Now:HH:mm}");
    }


    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
      ES_AWAYMODE_REQUIRED = 0x00000040,
      ES_CONTINUOUS = 0x80000000,
      ES_DISPLAY_REQUIRED = 0x00000002,
      ES_SYSTEM_REQUIRED = 0x00000001
      // Legacy flag, should not be used.
      // ES_USER_PRESENT = 0x00000004
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
  }
}