using System.Runtime.InteropServices;

namespace LogMonitorWpfApp.RdpSession;

internal class Insomniac // ~ does not let to sleep.
{
  readonly string _crlf = $" ", TextLog = @$"RdpFacility.{Environment.MachineName}.Log.txt";
  bool _isOn;

  internal void SetInso(bool isOn) { if (isOn) RequestActive(); else RequestRelease(); }

  internal void RequestActive(string crlf = " ")
  {
    _isOn = true;
    _ = SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
    //File.AppendAllText(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  Dr - On {crlf}");
    File.AppendAllText(TextLog, $"Dr-On{crlf}");
  }
  internal void RequestRelease(string crlf = " ")
  {
    if (!_isOn) return;

    _ = SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
    //File.AppendAllText(App.TextLog, $"{DateTimeOffset.Now:HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  Dr - Off {crlf}");
    File.AppendAllText(TextLog, $"Dr-Off{crlf}");
  }

  [Flags]
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