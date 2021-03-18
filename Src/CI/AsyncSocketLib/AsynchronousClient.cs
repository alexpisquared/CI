using AsyncSocketLib.CI.Model;
using AsyncSocketLib.Shared;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncSocketLib
{
  public class AsynchronousClient
  {
    readonly ManualResetEvent _connectDone = new(false), _sendingDone = new(false), _receiveDone = new(false);    // ManualResetEvent instances signal completion.  
    readonly DateTime _started;
    readonly object _theLock = new();
    unsafe ChangeResponse _changeResponse;
    unsafe LoginResponse _loginResponse;
    const int _delay = 50;
    string _report = "", _responseStrVer = ""; // The response from the remote device.  
    int _bytesReceived;

    public AsynchronousClient() => _started = DateTime.Now;

    public string Report { get => _report; set { lock (_theLock) { _report += value; } } }
    public LoginResponse LoginResponse => _loginResponse;
    public ChangeResponse ChangeResponse => _changeResponse;

    public async Task<string> SendLockOrder(string uri, int port, string username, int requestID, bool isLock)
    {
      try
      {
        var ipAddress = Dns.GetHostEntry(uri).AddressList[0];

        using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  
        client.BeginConnect(new IPEndPoint(ipAddress, port), new AsyncCallback(ConnectCallback), client);
        var sw = Stopwatch.StartNew();
        if (!_connectDone.WaitOne(_delay))
        {
          return Report = $"  Failed to connect in {sw.ElapsedMilliseconds} / {_delay} \n";
        }

        Report = $" ◄ {uri}:{port}  in {sw.ElapsedMilliseconds} / {_delay} ms\n";

        sendLoginRequestRmsC(client, username);
        _sendingDone.WaitOne();
        Receive<LoginResponse>(client);
        await Task.Delay(_delay);

        sendLockoRequestRmsC(client, requestID, isLock);
        _sendingDone.WaitOne();
        Receive<ChangeResponse>(client);
        await Task.Delay(_delay);

        sw = Stopwatch.StartNew();
        Report = $"  Finally, waiting {_delay} for the Response ...";
        _receiveDone.WaitOne(_delay);
        Report = $" in {sw.ElapsedMilliseconds} / {_delay} response of {_responseStrVer} received;  closing client socket.\n\n";

        client.Shutdown(SocketShutdown.Both);
        client.Close();
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }

      return Report;
    }

    public async Task<string> SendChangeRequest(string uri, int port, string username, int requestID, RequestStatus status, int doneQty, double price, string note)
    {
      try
      {
        var ipAddress = Dns.GetHostEntry(uri).AddressList[0];

        using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  
        client.BeginConnect(new IPEndPoint(ipAddress, port), new AsyncCallback(ConnectCallback), client);
        var sw = Stopwatch.StartNew();
        if (!_connectDone.WaitOne(_delay))
        {
          return Report = $"  Failed to connect in {sw.ElapsedMilliseconds} / {_delay} \n";
        }

        Report = $" ◄ {uri}:{port}  in {sw.ElapsedMilliseconds} / {_delay} ms\n";

        sendLoginRequestRmsC(client, username);
        _sendingDone.WaitOne();
        Receive<LoginResponse>(client);
        await Task.Delay(_delay);

        sendChngeRequestRmsC(client, requestID, status, doneQty, price, note);
        _sendingDone.WaitOne();
        Receive<ChangeResponse>(client);
        await Task.Delay(_delay);

        sw = Stopwatch.StartNew();
        Report = $"  Finally, waiting {_delay} for the Response ...";
        _receiveDone.WaitOne(_delay);
        Report = $" in {sw.ElapsedMilliseconds} / {_delay} response of {_responseStrVer} received;  closing client socket.\n\n";

        client.Shutdown(SocketShutdown.Both);
        client.Close();
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }

      return Report;
    }
    public async Task<string> ConnectSendClosePOC(string uri, int port, string username, string job)
    {
      try
      {
        var ipAddress = Dns.GetHostEntry(uri).AddressList[0];

        using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  
        client.BeginConnect(new IPEndPoint(ipAddress, port), new AsyncCallback(ConnectCallback), client);
        var sw = Stopwatch.StartNew();
        if (!_connectDone.WaitOne(_delay))
        {
          return Report = $"  Failed to connect in {sw.ElapsedMilliseconds} / {_delay} \n";
        }

        Report = $" ◄ {uri}:{port}  in {sw.ElapsedMilliseconds} / {_delay} ms\n";

        switch (job)
        {
          case "stringPoc": sendOriginalStr(client, "This is a TEST <EOF>"); _sendingDone.WaitOne(); Receive<UnknownType>(client); break;
          case "logInRmsC":
            sendLoginRequestRmsC(client, username); _sendingDone.WaitOne(); Receive<LoginResponse>(client);
            await Task.Delay(_delay);
            sendChngeRequestRmsC(client, 26859, RequestStatus.rsDone, 100, 0, $"C#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# z"); _sendingDone.WaitOne(); Receive<ChangeResponse>(client); await Task.Delay(_delay);
            sendChngeRequestRmsC(client, 26860, RequestStatus.rsDone, 008, 0, $"C#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# z"); _sendingDone.WaitOne(); Receive<ChangeResponse>(client); await Task.Delay(_delay);
            sendChngeRequestRmsC(client, 26861, RequestStatus.rsDone, 001, 0, $"C#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# z"); _sendingDone.WaitOne(); Receive<ChangeResponse>(client); await Task.Delay(_delay);
            sendChngeRequestRmsC(client, 26862, RequestStatus.rsDone, 000, 0, $"C#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# zC#C#C#C# z"); _sendingDone.WaitOne(); Receive<ChangeResponse>(client); await Task.Delay(_delay);
            break;
          case "logInRisk": sendLoginRequestRisk(client, username); _sendingDone.WaitOne(); Receive<RiskBaseMsg>(client); break;
          default: throw new Exception($"{job} is unknown");
        }

        sw = Stopwatch.StartNew();
        Report = $"  Finally, waiting {_delay} for the Response ...";
        _receiveDone.WaitOne(_delay);
        Report = $" in {sw.ElapsedMilliseconds} / {_delay} response of {_responseStrVer} received;  closing client socket.\n\n";

        client.Shutdown(SocketShutdown.Both);
        client.Close();
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }

      return Report;
    }
    public async Task ConnectSendClose_formerStartClient(string uri, int port) => await ConnectSendClosePOC(uri, port, "", "stringPoc");

    void ConnectCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState;   // Retrieve the socket from the state object.  
        client.EndConnect(ar);                // Complete the connection.  

        Report = ($"    ◄ Ccb  Socket connected to  {client.RemoteEndPoint}");

        _connectDone.Set();                   // Signal that the connection has been made.  
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
    }
    void Receive<T>(Socket client)
    {
      try
      {
        var state = new StateObject { WorkSocket = client };

        client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback<T>), state);
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
    }
    unsafe void ReceiveCallback<T>(IAsyncResult ar)
    {
      Report = ($"    ◄ Rcb ");
      try
      {
        var state = (StateObject)ar.AsyncState;        // Retrieve the state object and the client socket from the asynchronous state object.  
        var client = state.WorkSocket;

        Report = ($" {(client?.Connected == true ? "connected" : client?.Connected == false ? "disConnected" : "WorkSocket is NULL")} \t");
        if (client?.Connected == true)
        {
          var bytesRead = client?.EndReceive(ar) ?? 0; // Read data from the remote device.  
          if (bytesRead > 0)
          {
            state.SB.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));          // There might be more data, so store the data received so far.  

            Report = $"{state.SB.Length,4} bytes got so far; maybe more coming...";

            client?.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback<T>), state); // Get the rest of the data.  
          }
          _bytesReceived += bytesRead;
        }
        else
        {
          Report = $"{state.SB.Length,4} bytes - All the data has arrived";

          if (state.SB.Length > 1)          // All the data has arrived; put it in response.  
          {
            _responseStrVer = $"{state.SB.Length,4} bytes";
          }

          _receiveDone.Set();               // Signal that all bytes have been received.  
        }

        if (_bytesReceived >= sizeof(MessageHeader) && state?.Buffer != null)
        {
          fixed (byte* buffer = state.Buffer)
          {
            var header = (MessageHeader*)buffer;

            switch ((*header).m_type)
            {
              case MessageType.mtNotSet: break;
              case MessageType.mtResponse: break;
              case MessageType.mtLogin: break;
              case MessageType.mtChangeRequest: break;
              case MessageType.mtNewRequestNotification: break;
              case MessageType.mtLockOrder: break;
              case MessageType.mtLockOrderResponse: break;
              case MessageType.mtLoginResponse:  /**/{ _loginResponse = *((LoginResponse*)buffer); Report = $"\n      ■■■  Resp: {_loginResponse}·\n"; break; }
              case MessageType.mtChangeResponse: /**/{ _changeResponse = *((ChangeResponse*)buffer); Report = $"\n      ■■■  Resp: {_changeResponse}·\n"; break; }
              default: break;
            }

            if (_bytesReceived >= header->m_size)
            {
              var bytesProcessed = ProcessMessages(buffer, _bytesReceived);
              var remainder = _bytesReceived - bytesProcessed;
              if (remainder > 0)
              {
                Report = $" \t {remainder} > 0   =>   data moved!";
                BinaryHelper.MoveData(state.Buffer, bytesProcessed, remainder);
              }
              _bytesReceived -= bytesProcessed;
            }
          }
        }

        _responseStrVer = $"{state?.SB.Length,4} bytes";
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n"; }
      finally { Report = "\n"; }
    }

    unsafe int ProcessMessages(byte* buffer, int length)
    {
      var processedLength = 0;
      var remainderLength = length;
      while (true)
      {
        if (remainderLength < sizeof(MessageHeader))
          break;

        var messageHeader = (MessageHeader*)(buffer + processedLength);
        if (remainderLength < messageHeader->m_size)
          break;

        ProcessMessage(messageHeader);
        processedLength += messageHeader->m_size;
        remainderLength = length - processedLength;
      }

      return processedLength;
    }

    unsafe void ProcessMessage(MessageHeader* messageHeader)
    {
      switch (messageHeader->m_type)
      {
        case MessageType.mtLoginResponse:          /**/ ProcessMessage((LoginResponse*)messageHeader); break;
        case MessageType.mtChangeResponse:         /**/ ProcessMessage((ChangeResponse*)messageHeader); break;
        case MessageType.mtResponse:               /**/ ProcessMessage((Response*)messageHeader); break;
        case MessageType.mtNewRequestNotification: /**/ ProcessMessage((NewRequestNotification*)messageHeader); break;
        default: throw new Exception($"Message type  {messageHeader->m_type}  is unknown");
      }
    }
    unsafe void ProcessMessage(LoginResponse* response) => Debug.WriteLine($"Todo: m_mainForm.OnNewRequest({response->m_code})");
    unsafe void ProcessMessage(ChangeResponse* response) => Debug.WriteLine($"Todo: m_mainForm.OnNewRequest({response->m_code})");
    unsafe void ProcessMessage(NewRequestNotification* newRequestNotification) => Debug.WriteLine($"Todo: m_mainForm.OnNewRequest({newRequestNotification->m_requestID})");
    unsafe void ProcessMessage(Response* response)
    {
      switch (response->m_code)
      {
        case ResponseCode.rcUserNotFound: break;          //OnUserNotFound();
        case ResponseCode.rcChangeRequestError: break;
        case ResponseCode.rcOK: break;                    //m_mainForm.OnNewRequest(0);
        default: throw new Exception($"Response code  {response->m_code}  is unknown");
      }
    }

    void sendOriginalStr(Socket client, string stringData)
    {
      Report = $"  sending \t{stringData} ...\n";
      var byteData = Encoding.ASCII.GetBytes(stringData);
      send(client, byteData, byteData.Length);
    }
    unsafe void sendLoginRequestRmsC(Socket client, string username)
    {
      var sz = sizeof(LoginRequest);
      LoginRequest lr;
      lr.m_header.m_size = sz;
      lr.m_header.m_type = MessageType.mtLogin;
      lr.m_password[0] = 0;
      username.ToByteArray(lr.m_userName);

      var ptr = (byte*)&lr;
      var byteData = new byte[StateObject.BufferSize];
      for (var i = 0; i < sz; i++) byteData[i] = ptr[i];
      for (var i = 20; i < sz; i++) byteData[i] = 205; //mimic cpp: 

      //byteData = File.ReadAllBytes(@"C:\dev\trunk\Server\RMS\RMSClientCPP\Login.bin");

      Report = $"  ►{(DateTime.Now - _started):s\\.fff} sending \t{nameof(LoginRequest),-26} total: {sz}\n";             //

      //for (var i = 0; i < sz; i++) Debug.WriteLine($"  {i,4} {(byteData[i] == 0 ? "" : $"{(int)byteData[i],4}")}"); Debug.WriteLine($"-- total: { sz} for log-in request.");

      send(client, byteData, sz); // client.BeginSend(byteData, 0, sz, 0, new AsyncCallback(SendCallback), client);   
    }
    unsafe void sendLockoRequestRmsC(Socket client, int requestID, bool isLock)
    {
      const int sz = 13; // sizeof(LockOrderRequest);
      LockOrderRequest crm;
      crm.m_messageHeader.m_size = sz;
      crm.m_messageHeader.m_type = MessageType.mtLockOrder;
      crm.m_orderID = requestID;
      crm.m_isLock = isLock;

      var ptr = (byte*)&crm;
      var byteData = new byte[StateObject.BufferSize];
      for (var i = 0; i < sz; i++) byteData[i] = ptr[i];

      //byteData = File.ReadAllBytes(@"C:\dev\trunk\Server\RMS\RMSClientCPP\OrderUpdate.59.bin");

      Report = $"  ►{(DateTime.Now - _started):s\\.fff} sending \t{nameof(LockOrderRequest),-26} total: {sz}\n";             //

      //for (var i = 0; i < sz; i++) Debug.WriteLine($"  {i,4} {(byteData[i] == 0 ? "" : $"{(int)byteData[i],4}")}"); Debug.WriteLine($"-- total: { sz} for LockOrder request.");

      send(client, byteData, sz); // client.BeginSend(byteData, 0, sz, 0, new AsyncCallback(SendCallback), client);   
    }
    unsafe void sendChngeRequestRmsC(Socket client, int requestID, RequestStatus status, int doneQty, double price, string note)
    {
      const int sz = 153;
      ChangeRequest crm;
      crm.m_messageHeader.m_size = 153;
      crm.m_messageHeader.m_type = MessageType.mtChangeRequest;
      crm.m_data.m_type = (int)MessageType.mtResponse;
      crm.m_data.m_orderID = requestID;
      crm.m_data.m_parentID = 0;
      crm.m_data.m_status = (int)status;
      crm.m_data.m_lastShares = doneQty;
      crm.m_data.m_price = price;

      crm.m_data.m_int64Time = 132597104686937912; //todo: 
                                                   //crm.m_data.m_time.dwLowDateTime = 991704888;
                                                   //crm.m_data.m_time.dwHighDateTime = 30872669;

      note.ToByteArray(crm.m_data.m_bbsNote);

      var ptr = (byte*)&crm;
      var byteData = new byte[StateObject.BufferSize];
      for (var i = 0; i < sz; i++) byteData[i] = ptr[i];

      //byteData = File.ReadAllBytes(@"C:\dev\trunk\Server\RMS\RMSClientCPP\OrderUpdate.59.bin");

      Report = $"  ►{(DateTime.Now - _started):s\\.fff} sending \t{nameof(ChangeRequest),-26} total: {sz}\n";             //

      //for (var i = 0; i < sz; i++) Debug.WriteLine($"  {i,4} {(byteData[i] == 0 ? "" : $"{(int)byteData[i],4}")}"); Debug.WriteLine($"-- total: { sz} for change request.");

      send(client, byteData, sz); // client.BeginSend(byteData, 0, sz, 0, new AsyncCallback(SendCallback), client);   
    }
    unsafe void sendChangeRequestRmsC(Socket client, int requestID, RequestStatus status, uint doneQty, string note)
    {
      ChangeRequestMessage crm;
      crm.m_header.m_type = MessageType.mtChangeRequest;
      crm.m_header.m_size = sizeof(ChangeRequestMessage);
      crm.m_status = status;
      crm.m_doneQty = doneQty;
      crm.m_requestID = requestID;
      StringToByteArray(note, crm.m_bbsNote);

      var ptr = (byte*)&crm;
      var byteData = new byte[StateObject.BufferSize];
      for (var i = 0; i < sizeof(ChangeRequestMessage); i++) byteData[i] = ptr[i];

      Report = $"  ►{(DateTime.Now - _started):s\\.fff} sending \t{nameof(ChangeRequestMessage)} ...  -- size C# / C++: {sizeof(ChangeRequestMessage)} != ... \n";

      send(client, byteData, sizeof(ChangeRequestMessage)); // m_tcpClient.Send(m_sendBuffer, msg.m_header.m_size, SocketFlags.None);
    }
    unsafe void sendLoginRequestRisk(Socket client, string username)
    {
      RiskBaseMsg rbm;
      rbm.m_type = RiskMsgType.RISK_MSG_LOGON;
      rbm.m_seq = 0;
      rbm.m_size = 32;
      rbm.m_time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
      rbm.iGuidID = 101;

      var ptr = (byte*)&rbm;
      var byteData = new byte[StateObject.BufferSize];
      for (var i = 0; i < sizeof(RiskBaseMsg); i++) byteData[i] = ptr[i];

      Report = $"  ►{(DateTime.Now - _started):s\\.fff} sending \t{nameof(RiskBaseMsg)} ...  -- size C# / C++: {sizeof(RiskBaseMsg)} != {rbm.m_size}\n";

      send(client, byteData, rbm.m_size);
    }
    unsafe void StringToByteArray(string str, byte* buffer)
    {
      var bts = Encoding.ASCII.GetBytes(str);
      var len = str.Length;
      for (var i = 0; i < len; i++)
        buffer[i] = bts[i];

      buffer[len] = 0;
    }

    void send(Socket client, byte[] bytes, int len) => client.BeginSend(bytes, 0, len, 0, new AsyncCallback(sendCallback), client);
    void sendCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState; // Retrieve the socket from the state object.  
        var bytesSent = client.EndSend(ar); // Complete sending the data to the remote device.  
        Report = ($"    ◄ Scb  Sent{bytesSent,4}  bytes to server.\n");
        _sendingDone.Set();                 // Signal that all bytes have been sent.  
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
    }
  }
}