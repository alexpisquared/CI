using AsyncSocketLib.CI.Model;
using AsyncSocketLib.Shared;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace AsyncSocketLib
{
  public partial class AsynchronousClient
  {
    readonly ManualResetEvent _connectDone = new(false), _sendingDone = new(false), _receiveDone = new(false);    // ManualResetEvent instances signal completion.  
    readonly object _theLock = new();
    string _report = "", _responseStrVer = ""; // The response from the remote device.  
    RiskBaseMsg _rbm;
    int m_received;

    public string Report
    {
      get { try { return _report; } finally { /*_report = "";*/ } }
      set
      {
        lock (_theLock)
        {
          _report += value;
        }
      }
    }

    public void ConnectSendClose_formerStartClient(string uri, int port) => ConnectSendClose(uri, port, "");
    public void ConnectSendClose(string uri, int port, string username)
    {
      try
      {
        var ipHostInfo = Dns.GetHostEntry(uri);
        var ipAddress = ipHostInfo.AddressList[0];
        var remoteEP = new IPEndPoint(ipAddress, port);

        using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  
        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        Report = $"Trying to connect to \t{uri,44}:{port} ...\n";
        if (!_connectDone.WaitOne(2500))
        {
          Report = $"  Failed for 2.5 sec\n";
          return;
        }

        if (string.IsNullOrEmpty(username))
          sendOriginalStr(client, "This is a TEST <EOF>");
        else if (port == 6756)
        {
          sendLoginRequestRmsC(client, username);
          _sendingDone.WaitOne();
          Receive<UnknownType>(client); // Receive the response from the remote device.  
        }
        else if (port == 22225)
        {
          sendLoginRequestRisk(client, username);
          _sendingDone.WaitOne();
          Receive<RiskBaseMsg>(client); // Receive the response from the remote device.  
        }

        Report = "  waiting for the Response ...\n";
        _receiveDone.WaitOne(1000); // wait just for a sec

        Report = $"  response received: '{_responseStrVer}'.  Closing client socket.\n\n";

        client.Shutdown(SocketShutdown.Both);
        client.Close();
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
    }

    void ConnectCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState;   // Retrieve the socket from the state object.  
        client.EndConnect(ar);                // Complete the connection.  

        Report = ($"    <= Socket connected to \t{client.RemoteEndPoint,42} +++\n");

        _connectDone.Set();                   // Signal that the connection has been made.  
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
    }
    void Receive<T>(Socket client)
    {
      try
      {
        var state = new StateObject { workSocket = client };

        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback<T>), state);
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
    }
    unsafe void ReceiveCallback<T>(IAsyncResult ar)
    {
      Report = ($"    <= ");
      try
      {
        var state = (StateObject)ar.AsyncState;        // Retrieve the state object and the client socket from the asynchronous state object.  
        var client = state.workSocket;

        Report = ($"Connected:{client?.Connected}  ");
        if (client?.Connected == true)
        {
          var bytesRead = client?.EndReceive(ar) ?? 0; // Read data from the remote device.  
          if (bytesRead > 0)
          {
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));          // There might be more data, so store the data received so far.  

            Report = $"'{state.sb}' got so far; maybe more coming...";

            client?.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback<T>), state); // Get the rest of the data.  
          }
          m_received += bytesRead;
        }
        else
        {
          Report = $"'{state.sb}' All the data has arrived";

          if (state.sb.Length > 1)          // All the data has arrived; put it in response.  
          {
            _responseStrVer = state.sb.ToString();
          }

          _receiveDone.Set();               // Signal that all bytes have been received.  
        }

        //nogo: _rbm = FromByteArray<RiskBaseMsg>(state.buffer);
        if (m_received >= 12 && state?.buffer != null)  //sizeof(RiskBaseMsg))
        {
          fixed (byte* pSource = state.buffer)
          {
            var msgHeader = (RiskBaseMsg*)pSource;
            Report = $"\n       seq:{(*msgHeader).m_seq}  sz:{(*msgHeader).m_size}  t:{(*msgHeader).m_time}  gu:{(*msgHeader).iGuidID}  ty:{(RiskMsgType)(*msgHeader).m_type}";
            if (m_received >= msgHeader->m_size)
            {
              var bytesProcessed = ProcessMessages(pSource, m_received);
              var remainder = m_received - bytesProcessed;
              if (remainder > 0)
              {
                BinaryHelper.MoveData(state.buffer, bytesProcessed, remainder);
              }
              m_received -= bytesProcessed;
            }
          }
        }

        _responseStrVer = state?.sb.ToString() ?? "Emtpy as null can be";
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n"; }
      finally { Report = "\n"; }
    }

    unsafe int ProcessMessages(byte* buffer, int length)
    {
      var processed = 0;
      var remainder = length;
      for (; ; )
      {
        if (remainder < sizeof(MessageHeader))
        {
          break;
        }
        var msgHeader = (MessageHeader*)(buffer + processed);
        if (remainder < msgHeader->m_size)
        {
          break;
        }
        ProcessMessage(msgHeader);
        processed += msgHeader->m_size;
        remainder = length - processed;
      }
      return processed;
    }
    unsafe void ProcessMessage(MessageHeader* msg)
    {
      switch (msg->m_type)
      {
        case MessageType.mtResponse:
          ProcessResponse((Response*)msg);
          break;
        case MessageType.mtNewRequestNotification:
          ProcessNewRequestNotification((NewRequestNotification*)msg);
          break;

      }
    }
    unsafe void ProcessNewRequestNotification(NewRequestNotification* msg)
    {
      //m_mainForm.OnNewRequest(msg->m_requestID);
    }
    unsafe void ProcessResponse(Response* resp)
    {
      switch (resp->m_code)
      {
        case ResponseCode.rcUserNotFound:
          //OnUserNotFound();
          break;
        case ResponseCode.rcChangeRequestError:
          break;
        case ResponseCode.rcOK:
          //m_mainForm.OnNewRequest(0);
          break;
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
      var byteData = new byte[StateObject.BufferSize];

      LoginRequest lr;
      lr.m_header.m_size = sizeof(LoginRequest);
      lr.m_header.m_type = MessageType.mtLogin;
      lr.m_password[0] = 0;
      username.ToByteArray(lr.m_userName);

      var ptr = (byte*)&lr;
      for (var i = 0; i < sizeof(LoginRequest); i++) byteData[i] = ptr[i];    //mimic cpp: for (var i = 20; i < sizeof(LoginRequest); i++) byteData[i] = 205;

      Report = $"  sending \t{nameof(LoginRequest)} ...  -- total: {sizeof(LoginRequest)}\n";             // for (var i = 0; i < sizeof(LoginRequest); i++) Debug.WriteLine($"  {i,4} {(int)byteData[i],4}"); Debug.WriteLine($"-- total: {sizeof(LoginRequest)}:");

      send(client, byteData, sizeof(LoginRequest)); // client.BeginSend(byteData, 0, sizeof(LoginRequest), 0, new AsyncCallback(SendCallback), client); // Begin sending the data to the remote device.  
    }
    unsafe void sendLoginRequestRisk(Socket client, string username)
    {
      var byteData = new byte[StateObject.BufferSize];

      RiskBaseMsg rbm;
      rbm.m_type = RiskMsgType.RISK_MSG_LOGON;
      rbm.m_seq = 0;
      rbm.m_size = 32;
      rbm.m_time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
      rbm.iGuidID = 101;

      var ptr = (byte*)&rbm;
      for (var i = 0; i < sizeof(RiskBaseMsg); i++) byteData[i] = ptr[i];

      Report = $"  sending \t{nameof(RiskBaseMsg)} ...  -- size C# / C++: {sizeof(RiskBaseMsg)} != {rbm.m_size}\n";

      send(client, byteData, rbm.m_size);
    }

    void send(Socket client, byte[] bytes, int len) => client.BeginSend(bytes, 0, len, 0, new AsyncCallback(sendCallback), client); // Begin sending the data to the remote device.      
    void sendCallback(IAsyncResult ar)
    {
      try
      {
        var client = (Socket)ar.AsyncState; // Retrieve the socket from the state object.  
        var bytesSent = client.EndSend(ar); // Complete sending the data to the remote device.  
        Report = ($"    <= Sent  {bytesSent}  bytes to server.\n");
        _sendingDone.Set();                 // Signal that all bytes have been sent.  
      }
      catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
    }
  }
}