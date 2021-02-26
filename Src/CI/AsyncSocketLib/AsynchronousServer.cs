using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsyncSocketLib
{
  public class AsynchronousServer
  {
    public ManualResetEvent allDone = new ManualResetEvent(false); // Thread signal.  

    public AsynchronousServer()
    {
    }

    public void StartListening(string uri, int port)
    {
      // Establish the local endpoint for the socket.  
      // The DNS name of the computer  
      // running the listener is "host.contoso.com".  
      var ipHostInfo = Dns.GetHostEntry(uri);
      var ipAddress = ipHostInfo.AddressList[0];
      var localEndPoint = new IPEndPoint(ipAddress, port);

      // Create a TCP/IP socket.  
      var listener = new Socket(ipAddress.AddressFamily,
          SocketType.Stream, ProtocolType.Tcp);

      // Bind the socket to the local endpoint and listen for incoming connections.  
      try
      {
        listener.Bind(localEndPoint);
        listener.Listen(100);

        while (true)
        {
          // Set the event to nonsignaled state.  
          allDone.Reset();

          // Start an asynchronous socket to listen for connections.  
          Console.WriteLine($"Waiting for a connection on  {ipAddress}:{port}...");
          listener.BeginAccept(
              new AsyncCallback(AcceptCallback),
              listener);

          // Wait until a connection is made before continuing.  
          allDone.WaitOne();
        }

      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }

      Console.WriteLine("\nPress ENTER to continue...");
      Console.Read();

    }

    public void AcceptCallback(IAsyncResult ar)
    {
      // Signal the main thread to continue.  
      allDone.Set();

      // Get the socket that handles the client request.  
      var listener = (Socket)ar.AsyncState;
      var handler = listener.EndAccept(ar);

      // Create the state object.  
      var state = new StateObject
      {
        workSocket = handler
      };
      handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
          new AsyncCallback(ReadCallback), state);
    }

    public void ReadCallback(IAsyncResult ar)
    {
      var content = string.Empty;
      var state = (StateObject)ar.AsyncState;    // Retrieve the state object and the handler socket from the asynchronous state object.  
      var handler = state.workSocket;
      var bytesRead = handler.EndReceive(ar);    // Read data from the client socket.

      if (bytesRead > 0)
      {
        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));      // There  might be more data, so store the data received so far.  

        content = state.sb.ToString();      // Check for end-of-file tag. If it is not there, read more data.  
        if (content.IndexOf("<EOF>") > -1)
        {
          // All the data has been read from the client. Display it on the console.  
          Console.WriteLine("Read {0} bytes from socket: \n  {1}", content.Length, content);
          Send(handler, content);        // Echo the data back to the client.  
        }
        else
        {
          // Not all data received. Get more.  
          handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
          new AsyncCallback(ReadCallback), state);
        }
      }
    }

    void Send(Socket handler, string data)
    {
      // Convert the string data to byte data using ASCII encoding.  
      var byteData = Encoding.ASCII.GetBytes(data);

      // Begin sending the data to the remote device.  
      handler.BeginSend(byteData, 0, byteData.Length, 0,
          new AsyncCallback(SendCallback), handler);
    }

    void SendCallback(IAsyncResult ar)
    {
      try
      {
        // Retrieve the socket from the state object.  
        var handler = (Socket)ar.AsyncState;

        // Complete sending the data to the remote device.  
        var bytesSent = handler.EndSend(ar);
        Console.WriteLine("Sent {0} bytes to client.", bytesSent);

        handler.Shutdown(SocketShutdown.Both);
        handler.Close();

      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }
  }
}