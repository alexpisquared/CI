﻿
using System;
using System.Diagnostics;
using System.Security.Principal;

namespace AsLink
{
  [Obsolete(@"Use C:\g\AAV.Shared\Src\AsLink\PlatformNeutral\EvLogHelperBase.cs")]
  public static partial class EvLogHelper // a copy from org: C:\g\AAV.Shared\Src\AsLink\PlatformNeutral\EvLogHelperBase.cs
  {
    const string _app = "Application", _sec = "Security", _sys = "System", _aavSource = "AavSource", _aavLogName = "AavNewLog";
    public static string CheckCreateLogChannel(string src = _aavSource, string log = _aavLogName) // which one is the latest
    {
      if (!IsAdministrator())
        return ("Restart as Admin!!!  (...to query/create event log)");
      else
        return safeCreateEventSource(src, log);
    }
    public static bool IsAdministrator()
    {
      var identity = WindowsIdentity.GetCurrent();
      if (identity != null)
      {
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
      }

      return false;
    }
    public static void LogScrSvrBgn(int App_Ssto_Gp, string msg = "ScrSvr - Up. ScreenSaveTimeOut+GracePeriod ") { try { EventLog.WriteEntry(_aavSource, $"{msg}(sec) ={App_Ssto_Gp}", EventLogEntryType.Information, 7101); } catch (Exception ex) { ex.Log(); } }// AavSource finds the log named AavNewLog    
    public static void LogScrSvrEnd(DateTime actualIdleAt, int ssTimeoutSec, string msg) { try { EventLog.WriteEntry(_aavSource, $"Idle since {actualIdleAt:HH:mm:ss} for {(DateTime.Now - actualIdleAt):hh\\:mm\\:ss}  (including SSvr Timeout of {ssTimeoutSec} sec) -  {msg}", EventLogEntryType.Information, 7102); } catch (Exception ex) { ex.Log(); } }// AavSource finds the log named AavNewLog

    static string safeCreateEventSource(string src, string log)
    {
      if (!EventLog.SourceExists(src))
      {
        EventLog.CreateEventSource(new EventSourceCreationData(src, log)); // replacing: EventLog.CreateEventSource(src, log, Environment.MachineName);
        return "Created the log";
      }

      return $"{src}\\{log} already exists";
    }
  }
}