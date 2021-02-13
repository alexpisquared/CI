using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RdpFacility
{
  public partial class App : Application
  {
    public const string TextLog = @"C:\temp\EventLog.txt";
    public static DateTimeOffset Started; // lazy 
    protected override void OnStartup(StartupEventArgs e)
    {
      Started = DateTimeOffset.Now;
      base.OnStartup(e);
      Debug.WriteLine($"{Started:HH:mm:ss.fffffff}");
    }
  }
}
