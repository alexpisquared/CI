using System;
using System.Diagnostics;
using System.Windows;

namespace RdpFacility
{
  public partial class App : Application
  {
    public static readonly string TextLog; // = @"C:\temp\EventLog.txt";
    public static DateTimeOffset Started;  // lazy 

    static App() => TextLog = @$"RdpFacility.{Environment.MachineName}.Log.txt";
    protected override void OnStartup(StartupEventArgs e)
    {
      Started = DateTimeOffset.Now; // the soonest self awareness.
      base.OnStartup(e);
      Debug.WriteLine($"{Started:HH:mm:ss.fffffff}");
    }
  }
}
