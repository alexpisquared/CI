using AsyncSocketLib.CI.Model;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsyncSocketLib
{
  public partial class AsynchronousClient
  {
    readonly ManualResetEvent _connectDone = new ManualResetEvent(false); // ManualResetEvent instances signal completion.  
    readonly ManualResetEvent _sendingDone = new ManualResetEvent(false);
    readonly ManualResetEvent _receiveDone = new ManualResetEvent(false);
    string _report = "";
    string _response = string.Empty; // The response from the remote device.  

    public string Report => _report;

    public void StartClient(string uri, int port)
    {
      try
      {
        // Establish the remote endpoint for the socket.  
        var ipHostInfo = Dns.GetHostEntry(uri);
        var ipAddress = ipHostInfo.AddressList[0];
        var remoteEP = new IPEndPoint(ipAddress, port);

        using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        _report += ($"  Trying to connect to \t{uri}:{port} ...");
        _connectDone.WaitOne();

        sendOriginal(client, "This is a test<EOF>");
        _sendingDone.WaitOne();

        Receive(client); // Receive the response from the remote device.  
        _report += ("  Waiting for the Response ...");
        _receiveDone.WaitOne();

        _report += ($"  Response received : {_response}\n");

        client.Shutdown(SocketShutdown.Both);
        client.Close();
      }
      catch (Exception e) { _report += $"{e}\n\n"; }
    }
    public void StartClientReal(string uri, int port, string username)
    {
      try
      {
        // Establish the remote endpoint for the socket.  
        var ipHostInfo = Dns.GetHostEntry(uri);
        var ipAddress = ipHostInfo.AddressList[0];
        var remoteEP = new IPEndPoint(ipAddress, port);

        using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  

        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        _report += ($"  Trying to connect to \t{uri}:{port} ...");
        _connectDone.WaitOne();

        sendLoginRequest(client, username);
        _sendingDone.WaitOne();

        Receive(client); // Receive the response from the remote device.  
        _report += ("  Waiting for the Response ...");
        _receiveDone.WaitOne(1000); // wait just for a sec

        _report += ($"   Response received : '{_response}'");

        client.Shutdown(SocketShutdown.Both);
        client.Close();
      }
      catch (Exception e) { _report += $"{e}\n\n"; }
    }

    void ConnectCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState;   // Retrieve the socket from the state object.  
        client.EndConnect(ar);                // Complete the connection.  

        _report += ($"   Socket connected to \t{client.RemoteEndPoint} +++\n");

        _connectDone.Set();                   // Signal that the connection has been made.  
      }
      catch (Exception e) { _report += $"{e}\n\n"; }
    }
    void Receive(Socket client)
    {
      try
      {
        var state = new StateObject
        {
          workSocket = client
        };

        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
      }
      catch (Exception e) { _report += $"{e}\n\n"; }
    }
    void ReceiveCallback(IAsyncResult ar)
    {
      try
      {
        // Retrieve the state object and the client socket from the asynchronous state object.  
        var state = (StateObject)ar.AsyncState;
        var client = state.workSocket;

        var bytesRead = client?.EndReceive(ar) ?? 0; // Read data from the remote device.  
        if (bytesRead > 0)
        {
          // There might be more data, so store the data received so far.  
          state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

          // Get the rest of the data.  
          client?.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
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
      catch (Exception e) { _report += $"{e}\n\n"; }
    }

    void sendOriginal(Socket client, string data)
    {
      var byteData = Encoding.ASCII.GetBytes(data);

      send(client, byteData, byteData.Length);
    }
    unsafe void sendLoginRequest(Socket client, string username)
    {
      var byteData = new byte[StateObject.BufferSize];

      LoginRequest lr;
      lr.m_header.m_size = sizeof(LoginRequest);
      lr.m_header.m_type = MessageType.mtLogin;
      lr.m_password[0] = 0;
      username.ToByteArray(lr.m_userName);
#if EofDemo
      "<EOF>".ToByteArray(lr.m_eof);
#endif

      var ptr = (byte*)&lr;
      for (var i = 0; i < sizeof(LoginRequest); i++) byteData[i] = ptr[i];
      
      //mimic cpp: for (var i = 20; i < sizeof(LoginRequest); i++) byteData[i] = 205;

      Debug.WriteLine($"-- total: {sizeof(LoginRequest)}:"); // for (var i = 0; i < sizeof(LoginRequest); i++) Debug.WriteLine($"  {i,4} {(int)byteData[i],4}"); Debug.WriteLine($"-- total: {sizeof(LoginRequest)}:");

      send(client, byteData, sizeof(LoginRequest)); // client.BeginSend(byteData, 0, sizeof(LoginRequest), 0, new AsyncCallback(SendCallback), client); // Begin sending the data to the remote device.  
    }

    void send(Socket client, byte[] bytes, int len) => client.BeginSend(bytes, 0, len, 0, new AsyncCallback(sendCallback), client); // Begin sending the data to the remote device.      
    void sendCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState; // Retrieve the socket from the state object.  
        var bytesSent = client.EndSend(ar); // Complete sending the data to the remote device.  
        _report += ("  Sent {bytesSent} bytes to server.\n");
        _sendingDone.Set();                 // Signal that all bytes have been sent.  
      }
      catch (Exception e) { _report += $"{e}\n\n"; }
    }
  }
}