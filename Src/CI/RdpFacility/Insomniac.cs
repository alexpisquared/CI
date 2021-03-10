using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RdpFacility
{
  internal class Insomniac
  {
    bool _isOn = false;

        internal void SetInso(bool isOn) { if (isOn) RequestActive(); else RequestRelease(); }

        internal void RequestActive(string crlf = " ")
    {
      _isOn = true;
      SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
      //File.AppendAllText(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  Dr - On {crlf}");
      File.AppendAllText(App.TextLog, $"Dr-On{crlf}");
    }
    internal void RequestRelease(string crlf = " ")
    {
      if (!_isOn) return;

      SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
      //File.AppendAllText(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  Dr - Off {crlf}");
      File.AppendAllText(App.TextLog, $"Dr-Off{crlf}");
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