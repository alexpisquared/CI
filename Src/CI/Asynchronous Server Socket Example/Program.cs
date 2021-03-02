using System;
using System.Net;

await new AsyncSocketLib.AsynchronousServer().StartListening(Dns.GetHostName(), 11000, silentBeep);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("\nPress ENTER to continue...");
Console.Read();

void silentBeep()
{
  Console.ForegroundColor = ConsoleColor.Yellow;
  Console.Write("beep.. ");
  Console.ResetColor();
}