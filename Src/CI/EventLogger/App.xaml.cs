using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EventLogger
{
  public partial class App : Application
  {
    public const string TextLog = @"C:\temp\EventLog.txt";
    public static DateTimeOffset Started = DateTimeOffset.Now;
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      //File.AppendAllText(_textLog, $"\n{_started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs())}\n");
    }
  }
}
