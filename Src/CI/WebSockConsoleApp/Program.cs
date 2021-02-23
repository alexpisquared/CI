using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

//t1("10.10.19.152", 6756);
t2("mtdevwebts01.bbssecurities.com", 6756);

static void t1(string ip, int port)
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

static void t2(string ip, int port)
{
  Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("\n************************");
  byte[] bytes = new byte[1024];

  try
  {
    //IPAddress ipAddress = IPAddress.Parse(ip);
    IPAddress ipAddress = Dns.GetHostEntry(ip).AddressList.First(); // If a host has multiple addresses, you will get a list of addresses  
    IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

    Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);    // Create a TCP/IP  socket.    

    try
    {
      Console.ForegroundColor = ConsoleColor.Green; 
      
      sender.Connect(remoteEP);

      Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

      // Encode the data string into a byte array.    
      byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

      Console.Write("Sending the data through the socket ... ");
      int bytesSent = sender.Send(msg);
      Console.WriteLine($"{bytesSent} bytes Sent.    ");

      Console.Write("Receiving the response from the remote device ... ");
      int bytesRec = sender.Receive(bytes);
      Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

      sender.Shutdown(SocketShutdown.Both);
      sender.Close();

      Console.WriteLine("The End");
    }
    catch (ArgumentNullException ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
    catch (SocketException ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
    catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
  }
  catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine(ex); }
}
