using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RdpFacility
{
  public partial class App : Application
  {
    public const string TextLog = @"C:\temp\EventLog.txt";
    public static DateTime Started = DateTime.Now;
  }
}
