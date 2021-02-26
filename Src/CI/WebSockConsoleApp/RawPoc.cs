using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RawPoc
{
  public static void t1(string ip, int port)
  {
    Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("\n************************");

    try
    {
      var sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      sck.Connect(ip, port);

      var msgbfr = Encoding.Default.GetBytes("Test");
      sck.Send(msgbfr, 0, msgbfr.Length, 0);

      var buffer = new byte[1024];
      var rec = sck.Receive(buffer, 0, buffer.Length, 0);
      Array.Resize(ref buffer, rec);

      Console.WriteLine($" Recieved: {Encoding.Default.GetString(buffer)}");

      sck.Close();
    }
    catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
  }

  public static void t2(string uri, int port)
  {
    Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("\n************************");
    var bytes = new byte[1024];

    try
    {
      //IPAddress ipAddress = IPAddress.Parse(ip);
      var endpoint = new IPEndPoint(Dns.GetHostEntry(uri).AddressList.First(), port);    // If a host has multiple addresses, you will get a list of addresses  

      using var senderSocket = new Socket(Dns.GetHostEntry(uri).AddressList.First().AddressFamily, SocketType.Stream, ProtocolType.Tcp);    // Create a TCP/IP  socket.    

      try
      {
        Console.ForegroundColor = ConsoleColor.Green;

        senderSocket.Connect(endpoint);

        Console.WriteLine("Socket connected to {0}", senderSocket.RemoteEndPoint.ToString());

        var msg = Encoding.ASCII.GetBytes("This is a test<EOF>"); // Encode the data string into a byte array.    

        Console.Write("Sending the data through the socket ... ");
        var bytesSent = senderSocket.Send(msg);
        Console.WriteLine($"{bytesSent} bytes Sent.    ");

        Console.Write("Receiving the response from the remote device ... ");
        var bytesRec = senderSocket.Receive(bytes);
        Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

        senderSocket.Shutdown(SocketShutdown.Both);
        senderSocket.Close();

        Console.WriteLine("The End");
      }
      catch (ArgumentNullException ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
      catch (SocketException ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
      catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
    }
    catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
  }
}