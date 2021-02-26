using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncSocketLib
{
  public partial class AsynchronousClient
  {
    // ManualResetEvent instances signal completion.  
    static readonly ManualResetEvent _connectDone = new ManualResetEvent(false);
    static readonly ManualResetEvent _sendingDone = new ManualResetEvent(false);
    static readonly ManualResetEvent _receiveDone = new ManualResetEvent(false);

    static string _response = string.Empty; // The response from the remote device.  

   public static void StartClient(string uri, int port)
    {
      try
      {
        // Establish the remote endpoint for the socket.  
        var ipHostInfo = Dns.GetHostEntry(uri);
        var ipAddress = ipHostInfo.AddressList[0];
        var remoteEP = new IPEndPoint(ipAddress, port);

        using (var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)) // Create a TCP/IP socket.  
        {
          client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
          Console.WriteLine($"  Trying to connect to {uri}:{port} ...");
          _connectDone.WaitOne();

          Send(client, "This is a test<EOF>");
          _sendingDone.WaitOne();

          Receive(client); // Receive the response from the remote device.  
          Console.WriteLine("  Waiting for the Response ...");
          _receiveDone.WaitOne();

          Console.WriteLine("  Response received : {0}", _response);

          client.Shutdown(SocketShutdown.Both);
          client.Close();
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }
    public static void StartClientReal(string uri, int port, string username)
    {
      try
      {
        // Establish the remote endpoint for the socket.  
        var ipHostInfo = Dns.GetHostEntry(uri);
        var ipAddress = ipHostInfo.AddressList[0];
        var remoteEP = new IPEndPoint(ipAddress, port);

        using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  

        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        Console.WriteLine($"  Trying to connect to {uri}:{port} ...");
        _connectDone.WaitOne();

        //SendLoginRequest(client, username);
        _sendingDone.WaitOne();

        Receive(client); // Receive the response from the remote device.  
        Console.WriteLine("  Waiting for the Response ...");
        _receiveDone.WaitOne();

        Console.WriteLine("  Response received : {0}", _response);

        client.Shutdown(SocketShutdown.Both);
        client.Close();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    static void ConnectCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState;   // Retrieve the socket from the state object.  
        client.EndConnect(ar);                // Complete the connection.  

        Console.WriteLine("  Socket connected to  {0} +++", client.RemoteEndPoint.ToString());

        _connectDone.Set();                   // Signal that the connection has been made.  
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    static void Receive(Socket client)
    {
      try
      {
        var state = new StateObject
        {
          workSocket = client
        };

        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    static void ReceiveCallback(IAsyncResult ar)
    {
      try
      {
        // Retrieve the state object and the client socket from the asynchronous state object.  
        var state = (StateObject)ar.AsyncState;
        var client = state.workSocket;

        var bytesRead = client.EndReceive(ar); // Read data from the remote device.  
        if (bytesRead > 0)
        {
          // There might be more data, so store the data received so far.  
          state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

          // Get the rest of the data.  
          client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }
        else
        {
          // All the data has arrived; put it in response.  
          if (state.sb.Length > 1)
          {
            _response = state.sb.ToString();
          }
          // Signal that all bytes have been received.  
          _receiveDone.Set();
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    static void Send(Socket client, string data)
    {
      var byteData = Encoding.ASCII.GetBytes(data);

      client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client); // Begin sending the data to the remote device.  
    }
    //static unsafe void SendLoginRequest(Socket client, string username)
    //{
    //  var byteData = new byte[BufferSize];
    //  LoginRequest lr;
    //  lr.m_header.m_type = MessageType.mtLogin;
    //  lr.m_header.m_size = sizeof(LoginRequest);
    //  lr.m_header.m_seqNo = ++m_seqNo;
    //  lr.m_password[0] = 0;
    //  StringToByteArray(username, lr.m_userName);
    //  StringToByteArray("<EOF>", lr.m_eof);

    //  var ptr = (byte*)&lr;
    //  for (var i = 0; i < sizeof(LoginRequest); i++) byteData[i] = ptr[i];

    //  client.BeginSend(byteData, 0, sizeof(LoginRequest), 0, new AsyncCallback(SendCallback), client); // Begin sending the data to the remote device.  
    //}

    static void SendCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState; // Retrieve the socket from the state object.  
        var bytesSent = client.EndSend(ar); // Complete sending the data to the remote device.  
        Console.WriteLine("  Sent {0} bytes to server.", bytesSent);
        _sendingDone.Set();                 // Signal that all bytes have been sent.  
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }
  }
}