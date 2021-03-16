using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApp1
{
  class Program
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine("Hello World!");

      var s = new AsyncSocketLib.AsynchronousClient();
      await s.ConnectSendClose_formerStartClient(Dns.GetHostName(), 11000);

      Console.WriteLine($"Report: {s.Report.Length}:\n{s.Report}");

      //StartClientReal(Dns.GetHostName(), 11000, "alex.pigida");
      //StartClientReal("10.10.19.152", 6756, "alex.pigida");

      Console.WriteLine("Hello World!");
    }
  }
}