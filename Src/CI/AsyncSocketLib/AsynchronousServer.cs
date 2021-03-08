using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncSocketLib
{
  public class AsynchronousServer : IDisposable
  {
    readonly ManualResetEvent _allDone = new(false); // Thread signal.  
    Socket? _listener;
    string _report = "";
    bool _keepOnListening = true;
    readonly int _blockingMs, _uiFreedomMs;

    public AsynchronousServer(int blockingMs = 100, int uiFreedomMs = 900) => (_blockingMs, _uiFreedomMs) = (blockingMs, uiFreedomMs);

    public async Task<string> StartListening(string uri, int port, Action beep)
    {
      var ipHostInfo = Dns.GetHostEntry(uri);
      var ipAddress = ipHostInfo.AddressList[0];
      var localEndPoint = new IPEndPoint(ipAddress, port); // Establish the local endpoint for the socket. ?? The DNS name of the computer running the listener is "host.contoso.com".  

      _listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  

      try
      {
        _listener.Bind(localEndPoint);      // Bind the socket to the local endpoint and listen for incoming connections.  
        _listener.Listen(100);

        while (_keepOnListening)
        {
          _allDone.Reset();    // Set the event to nonsignaled state.  

          _report += ($"\nStart an asynchronous socket to listen for connections.  \n  waiting for a connection on  {ipAddress}:{port}...\n");
          _listener.BeginAccept(new AsyncCallback(AcceptCallback), _listener);

          while (_keepOnListening && !_allDone.WaitOne(_blockingMs)) // Wait until a connection is made before continuing.  
          {
            beep();
            await Task.Delay(_uiFreedomMs);
          }
        }
      }
      catch (Exception e) { _report += $" ■ ■ ■ {e}\n\n"; }

      return _report;
    }
    public void StopAndClose() => _keepOnListening = false;
    public string Report { get { try { return _report; } finally { _report = ""; } } }

    public bool KeepOnListening { get => _keepOnListening; set => _keepOnListening = value; }

    void AcceptCallback(IAsyncResult ar)
    {
      _allDone.Set();      // Signal the main thread to continue.  

      var listener = (Socket)ar.AsyncState;      // Get the socket that handles the client request.  
      var handler = listener.EndAccept(ar);

      var state = new StateObject
      {
        WorkSocket = handler
      };

      handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
    }
    void ReadCallback(IAsyncResult ar)
    {
      var content = string.Empty;
      var state = (StateObject)ar.AsyncState;                                           // Retrieve the state object and the handler socket from the asynchronous state object.  
      var handler = state.WorkSocket;
      var bytesRead = handler?.EndReceive(ar) ?? 0;                                     // Read data from the client socket.

      if (bytesRead > 0)
      {
        state.SB.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));          // There  might be more data, so store the data received so far.  

        content = state.SB.ToString();                                                  // Check for end-of-file tag. If it is not there, read more data.  
        if (content.IndexOf("<EOF>") > -1)
        {
          _report += $"  Read {content.Length} bytes from socket: \n    {content} \n";  // All the data has been read from the client. Display it on the console.  
          Send(handler, content);                                                       // Echo the data back to the client.  
        }
        else
        {
          handler?.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,             // Not all data received. Get more.  
          new AsyncCallback(ReadCallback), state);
        }
      }
    }
    void Send(Socket? handler, string data)
    {
      var byteData = Encoding.ASCII.GetBytes(data);      // Convert the string data to byte data using ASCII encoding.  
      handler?.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);   
    }
    void SendCallback(IAsyncResult ar)
    {
      try
      {
        var handler = (Socket)ar.AsyncState;        // Retrieve the socket from the state object.  
        var bytesSent = handler.EndSend(ar);        // Complete sending the data to the remote device.  

        _report += $"  Sent {bytesSent} bytes to client.\n";

        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
      }
      catch (Exception e) { _report += $" ■ ■ ■ {e}\n\n"; }
    }

    void IDisposable.Dispose() => ((IDisposable?)_listener)?.Dispose();
  }
}