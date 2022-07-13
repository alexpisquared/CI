using System.Diagnostics;
using WindowsFormsLib;

namespace TextSenderWinFormsApp;

internal static class Program
{
  [STAThread]
  static void Main()
  {
    if (Debugger.IsAttached)
      new TextSender().SendPOC();
    else
    {
      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();
      Application.Run(new Form1());
    }
  }
}