using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;

namespace RMSClient.Comm
{
  public class ServerSession
  {
    readonly object m_thisLock = new object(); // public object CriticalSection => m_thisLock;
    readonly Socket m_tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    readonly byte[] m_recvBuffer = new byte[BufferSize];
    readonly byte[] m_sendBuffer = new byte[BufferSize];
    readonly HashSet<int> m_outstandingRequests = new HashSet<int>();
    RmsClientMainWindow m_mainForm;
    const int BufferSize = 1000;
    int m_received = 0;
    uint m_seqNo = 0;
    readonly ILogger<RmsClientMainWindow> _logger;

    public ServerSession(ILogger<RmsClientMainWindow> logger) => _logger = logger;

    class Request
    {
      public unsafe Request(MessageHeader* msgHeader) => m_msgHeader = msgHeader;
      public unsafe MessageHeader* m_msgHeader = null;
    }
    public enum MessageType
    {
      mtNotSet = 0,
      mtResponse = 1,
      mtLogin = 2,
      mtChangeRequest = 3,
      mtNewRequestNotification = 4
    };
    public enum ResponseCode
    {
      rcNotSet = -1,
      rcOK = 0,
      rcUserNotFound = 1,
      rcNotLoggedIn = 2,
      rcInternalError = 3,
      rcChangeRequestError = 4
    }
    public enum RequestStatus
    {
      rsSent = 1,
      rcReceived = 2,
      rsProcessing = 3,
      rsRejected = 4,
      rsCancelled = 5,
      rsPartialyDone = 6,
      rsDone = 7
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MessageHeader
    {
      public int m_size;
      public MessageType m_type;
      public uint m_seqNo;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Response
    {
      public MessageHeader m_header;
      public ResponseCode m_code;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct LoginRequest
    {
      public MessageHeader m_header;
      public fixed byte m_userName[128];
      public fixed byte m_password[32];
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NewRequestNotification
    {
      public MessageHeader m_header;
      public int m_requestID;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ChangeRequestMessage //todo: !!! see more at   $\\trunk\Server\RMS\Common\RMSMessage.h:170 - struct ChangeRequest  +  RMS.ChangeRequest structure.png here.
    {
      public MessageHeader m_header;
      public int m_requestID;
      public RequestStatus m_status;
      public uint m_doneQty;
      public fixed byte m_bbsNote[100];
    };
#if !!!DEBUG_UNIT_TEST
    public unsafe struct ChangeRequest
    {
      public MessageHeader m_messageHeader;
      public OrderUpdateData m_data;
    };
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct OrderUpdateData
    {
      [FieldOffset(sizeof(int) * 0)] public int m_updateID;
      [FieldOffset(sizeof(int) * 1)] public int /*UpdateType*/ m_type;
      [FieldOffset(sizeof(int) * 2)] public int m_orderID;
      [FieldOffset(sizeof(int) * 3)] public int m_parentID;
      [FieldOffset(sizeof(int) * 4)] public int /*OrderStatus*/ m_status;
      [FieldOffset(sizeof(int) * 5)] public int m_lastShares;
      [FieldOffset(sizeof(int) * 6)] public double m_price;
      [FieldOffset(sizeof(int) * 6 + sizeof(double))] public int m_userID;
      //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)]
      [FieldOffset(sizeof(int) * 7 + sizeof(double))] public fixed char/*BBSNote*/ m_bbsNote[101];
      //union 
      [FieldOffset(sizeof(int) * 7 + sizeof(double) + sizeof(char) * 101)] public System.Runtime.InteropServices.ComTypes.FILETIME m_time;
      [FieldOffset(sizeof(int) * 7 + sizeof(double) + sizeof(char) * 101)] public Int64  /*long long*/ m_int64Time;
    };
#endif

    public void SetMainForm(RmsClientMainWindow form) => m_mainForm = form;
    unsafe void StringToByteArray(string str, byte* buffer)
    {
      var b = System.Text.Encoding.ASCII.GetBytes(str);
      var len = str.Length;
      for (var i = 0; i < len; i++)
      {
        buffer[i] = b[i];
      }
      buffer[len] = 0;
    }

    string GetUserName() => WindowsIdentity.GetCurrent().Name.Split('\\').Last();
    public unsafe void LogIn()
    {
      LoginRequest lr;
      lr.m_header.m_type = MessageType.mtLogin;
      lr.m_header.m_size = sizeof(LoginRequest);
      lr.m_header.m_seqNo = ++m_seqNo;
      lr.m_password[0] = 0;
      StringToByteArray(GetUserName(), lr.m_userName);
      var p = (byte*)&lr;
      for (var i = 0; i < sizeof(LoginRequest); i++) m_sendBuffer[i] = p[i];

      var bytesSent = m_tcpClient.Send(m_sendBuffer, lr.m_header.m_size, SocketFlags.None);
      var asyncRslt = m_tcpClient.BeginReceive(m_recvBuffer, m_received, BufferSize - m_received, SocketFlags.None, new AsyncCallback(ReceiveData), m_tcpClient);
      logInfo($"Log In {bytesSent,4} bytes sent", asyncRslt);
    }
    unsafe void ReceiveData(IAsyncResult asyncResult)
    {
      logInfo($"ReceiveData( ...", asyncResult);
      try
      {
        var remote = (Socket)asyncResult.AsyncState;
        var recv = remote.EndReceive(asyncResult);
        if (recv == 0)
        {
          recv = 0;
        }

        m_received += recv;

        if (m_received >= sizeof(MessageHeader))
        {
          fixed (byte* pSource = m_recvBuffer)
          {
            var msgHeader = (MessageHeader*)pSource;
            if (m_received >= msgHeader->m_size)
            {
              var bytesProcessed = ProcessMessages(pSource, m_received);
              var remainder = m_received - bytesProcessed;
              if (remainder > 0)
              {
                MoveData(m_recvBuffer, bytesProcessed, remainder);
              }
              m_received -= bytesProcessed;
            }
          }
        }

        var rv = m_tcpClient.BeginReceive(m_recvBuffer, m_received, BufferSize - m_received, SocketFlags.None, new AsyncCallback(ReceiveData), m_tcpClient);
        _logger.LogInformation($" ■ ■ ■ ReceiveData() - in.IsCompleted:{asyncResult.IsCompleted}      out.IsCompleted:{rv.IsCompleted}.");
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    void SendData(IAsyncResult iar)
    {
      var remote = (Socket)iar.AsyncState;
      var sent = remote.EndSend(iar);
      m_received = 0;
      remote.BeginReceive(m_recvBuffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(ReceiveData), remote);
    }
    public void Connect(string address, ushort port)
    {
      _logger.LogInformation($" ■ ■ ■ Connect({address}:{port}) ..."); // Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      m_tcpClient.BeginConnect(new IPEndPoint(IPAddress.Parse(address), port), new AsyncCallback(Connected), m_tcpClient);
    }
    void Connected(IAsyncResult asyncRslt)
    {
      var socket = (Socket)asyncRslt.AsyncState;
      try
      {
        socket.EndConnect(asyncRslt);
        logInfo("Connected()  Logging in ...", asyncRslt);

        //socket.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), socket);

        LogIn();
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    void logInfo(string s, IAsyncResult asyncRslt) => _logger.LogInformation($" ■ ■ ■ {s,-40} Connected:{((Socket)asyncRslt.AsyncState).Connected,-5}    {((Socket)asyncRslt.AsyncState).RemoteEndPoint}   IsCompleted:{asyncRslt.IsCompleted,-5} .");
    static void MoveData(byte[] buffer, int offset, int size)
    {
      for (var i = 0; i < size; i++)
      {
        buffer[i] = buffer[i + offset];
      }
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
        case MessageType.mtResponse:                 /**/ ProcessResponse((Response*)msg); break;
        case MessageType.mtNewRequestNotification:   /**/ ProcessNewRequestNotification((NewRequestNotification*)msg); break;
        default: _logger.LogWarning($" ▄▀▄▀▄ seq:{msg->m_seqNo,3}    sz:{msg->m_size,5}    {msg->m_type,5} - unknown MessageType"); return;
      }
      _logger.LogInformation($" ■▓■▓■ seq:{msg->m_seqNo,3}    sz:{msg->m_size,5}    {msg->m_type,5} - {(MessageType)msg->m_type} ");
    }
    unsafe void ProcessNewRequestNotification(NewRequestNotification* msg) => m_mainForm.OnNewRequest(msg->m_requestID);
    unsafe void ProcessResponse(Response* resp)
    {
      switch (resp->m_code)
      {
        case ResponseCode.rcUserNotFound:
          MessageBox.Show($"User {GetUserName()} is not allowed to login to RMS.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          throw new System.Exception(null);
        case ResponseCode.rcChangeRequestError:
          break;
        case ResponseCode.rcOK:
          m_mainForm.OnNewRequest(0);
          break;
      }
    }
    public unsafe void SendChangeRequest(int requestID, string status, uint doneQty, string note)
    {
      ChangeRequestMessage msg;
      msg.m_header.m_type = MessageType.mtChangeRequest;
      msg.m_header.m_size = sizeof(ChangeRequestMessage);
      msg.m_status = m_mainForm.GetStatusID(status);
      msg.m_doneQty = doneQty;
      msg.m_requestID = requestID;
      StringToByteArray(note, msg.m_bbsNote);
      var p = (byte*)&msg;
      for (var i = 0; i < sizeof(ChangeRequestMessage); i++)              m_sendBuffer[i] = p[i];      

      var bytesSent = m_tcpClient.Send(m_sendBuffer, msg.m_header.m_size, SocketFlags.None);
      var asyncRslt = m_tcpClient.BeginReceive(m_recvBuffer, m_received, BufferSize - m_received, SocketFlags.None, new AsyncCallback(ReceiveData), m_tcpClient);

      logInfo($"SendCR {bytesSent,4} bytes sent", asyncRslt);
    }
  }
}
