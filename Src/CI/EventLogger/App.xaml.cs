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
    public const string _textLog = @"C:\temp\EventLog.txt";
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      File.AppendAllText(_textLog, $"\n{DateTime.Now:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs())}\n");
    }
  }
}
