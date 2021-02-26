using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class StateObject // State object for receiving data from remote device.  
{
  // Client socket.  
  public Socket workSocket = null;
  // Size of receive buffer.  
  public const int BufferSize = 256;
  // Receive buffer.  
  public byte[] buffer = new byte[BufferSize];
  // Received data string.  
  public StringBuilder sb = new StringBuilder();
}

public class AsynchronousClient
{
  // ManualResetEvent instances signal completion.  
  static readonly ManualResetEvent _connectDone = new ManualResetEvent(false);
  static readonly ManualResetEvent _sendingDone = new ManualResetEvent(false);
  static readonly ManualResetEvent _receiveDone = new ManualResetEvent(false);

  static string _response = string.Empty; // The response from the remote device.  

  static void StartClient(string uri, int port)
  {
    try
    {
      // Establish the remote endpoint for the socket.  
      var ipHostInfo = Dns.GetHostEntry(uri);
      var ipAddress = ipHostInfo.AddressList[0];
      var remoteEP = new IPEndPoint(ipAddress, port);

      using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  

      client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
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
    catch (Exception e)
    {
      Console.WriteLine(e.ToString());
    }
  }

  static void ConnectCallback(IAsyncResult ar)
  {
    try
    {
      // Retrieve the socket from the state object.  
      var client = (Socket)ar.AsyncState;

      // Complete the connection.  
      client.EndConnect(ar);

      Console.WriteLine("Socket connected to {0}",
          client.RemoteEndPoint.ToString());

      // Signal that the connection has been made.  
      _connectDone.Set();
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
      // Create the state object.  
      var state = new StateObject
      {
        workSocket = client
      };

      // Begin receiving the data from the remote device.  
      client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
          new AsyncCallback(ReceiveCallback), state);
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
      // Retrieve the state object and the client socket
      // from the asynchronous state object.  
      var state = (StateObject)ar.AsyncState;
      var client = state.workSocket;

      // Read data from the remote device.  
      var bytesRead = client.EndReceive(ar);

      if (bytesRead > 0)
      {
        // There might be more data, so store the data received so far.  
        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

        // Get the rest of the data.  
        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReceiveCallback), state);
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

  static void SendCallback(IAsyncResult ar)
  {
    try
    {
      // Retrieve the socket from the state object.  
      var client = (Socket)ar.AsyncState;

      // Complete sending the data to the remote device.  
      var bytesSent = client.EndSend(ar);
      Console.WriteLine("  Sent {0} bytes to server.", bytesSent);

      // Signal that all bytes have been sent.  
      _sendingDone.Set();
    }
    catch (Exception e)
    {
      Console.WriteLine(e.ToString());
    }
  }

  public static int Main(string[] args)
  {
    //StartClient(Dns.GetHostName(), 11000);
    StartClient("10.10.19.152", 6756);

    return 0;
  }
}