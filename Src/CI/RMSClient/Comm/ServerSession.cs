using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RMSClient.Comm
{
  public class ServerSession
  {
    Socket m_tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    uint m_seqNo = 0;
    Object m_thisLock = new Object();
    public Object CriticalSection
    {
      get { return m_thisLock; }
    }
    const int BufferSize = 1000;
    byte[] m_recvBuffer = new byte[BufferSize];
    byte[] m_sendBuffer = new byte[BufferSize];
    int m_received = 0;
    HashSet<int> m_outstandingRequests = new HashSet<int>();
    RmsClientMainWindow m_mainForm;

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
    unsafe public struct MessageHeader
    {
      public int m_size;
      public MessageType m_type;
      public uint m_seqNo;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct Response
    {
      public MessageHeader m_header;
      public ResponseCode m_code;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct LoginRequest
    {
      public MessageHeader m_header;
      public fixed byte m_userName[128];
      public fixed byte m_password[32];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct NewRequestNotification
    {
      public MessageHeader m_header;
      public int m_requestID;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct ChangeRequestMessage
    {
      public MessageHeader m_header;
      public int m_requestID;
      public RequestStatus m_status;
      public uint m_doneQty;
      public fixed byte m_bbsNote[100];
    };

    class Request
    {
      unsafe public Request(MessageHeader* msgHeader)
      {
        m_msgHeader = msgHeader;
      }
      unsafe public MessageHeader* m_msgHeader = null;
    }

    public void SetMainForm(RmsClientMainWindow form)
    {
      m_mainForm = form;
    }

    unsafe void StringToByteArray(String str, byte* buffer)
    {
      byte[] b = System.Text.Encoding.ASCII.GetBytes(str);
      int len = str.Length;
      for (int i = 0; i < len; i++)
      {
        buffer[i] = b[i];
      }
      buffer[len] = 0;
    }

    void Recv()
    {
      //try
      //{
      m_tcpClient.BeginReceive(m_recvBuffer, m_received, BufferSize - m_received, SocketFlags.None, new AsyncCallback(ReceiveData), m_tcpClient);
      //}
      //catch (SocketException e)
      //{
      //    string text = e.Message;
      //}
    }

    String GetUserName()
    {
      return WindowsIdentity.GetCurrent().Name.Split('.')[1];
    }



    unsafe public void LogIn()
    {
      LoginRequest lr;
      lr.m_header.m_type = MessageType.mtLogin;
      lr.m_header.m_size = sizeof(LoginRequest);
      lr.m_header.m_seqNo = ++m_seqNo;
      String userName = GetUserName();
      lr.m_password[0] = 0;
      StringToByteArray(userName, lr.m_userName);
      byte* p = (byte*)&lr;
      for (int i = 0; i < sizeof(LoginRequest); i++)
      {
        m_sendBuffer[i] = p[i];
      }
      m_tcpClient.Send(m_sendBuffer, lr.m_header.m_size, SocketFlags.None);
      Recv();
    }


    unsafe void ReceiveData(IAsyncResult iar)
    {
      try
      {
        Socket remote = (Socket)iar.AsyncState;
        int recv = remote.EndReceive(iar);
        if (recv == 0)
        {
          recv = 0;
        }
        m_received += recv;


        if (m_received >= sizeof(MessageHeader))
        {
          fixed (byte* pSource = m_recvBuffer)
          {
            MessageHeader* msgHeader = (MessageHeader*)pSource;
            if (m_received >= msgHeader->m_size)
            {
              int bytesProcessed = ServerSession.Instance.ProcessMessages(pSource, m_received);
              int remainder = m_received - bytesProcessed;
              if (remainder > 0)
              {
                MoveData(m_recvBuffer, bytesProcessed, remainder);
              }
              m_received -= bytesProcessed;
            }
          }
        }
        Recv();
      }
      catch (System.Exception ex)
      {

      }
    }

    void SendData(IAsyncResult iar)
    {
      Socket remote = (Socket)iar.AsyncState;
      int sent = remote.EndSend(iar);
      //m_received = 0;
      //remote.BeginReceive(m_recvBuffer, 0, BufferSize, SocketFlags.None, new AsyncCallback(ReceiveData), remote);
    }


     ServerSession()
    {
    }

    public static readonly ServerSession Instance = new ServerSession();

    public void Connect(String address, ushort port)
    {
      //Socket newsock = new Socket(AddressFamily.InterNetwork,
      //                SocketType.Stream, ProtocolType.Tcp);
      IPEndPoint iep = new IPEndPoint(IPAddress.Parse(address), port);
      m_tcpClient.BeginConnect(iep, new AsyncCallback(Connected), m_tcpClient);
    }

    void Connected(IAsyncResult iar)
    {
      Socket s = (Socket)iar.AsyncState;
      //try
      //{
      s.EndConnect(iar);
      //conStatus.Text = "Connected to: " + client.RemoteEndPoint.ToString();
      //client.BeginReceive(data, 0, size, SocketFlags.None,
      //              new AsyncCallback(ReceiveData), client);
      LogIn();
      //}
      //catch (SocketException e)
      //{
      //    String str = e.Message;
      //}
    }






    static void MoveData(byte[] buffer, int offset, int size)
    {
      for (int i = 0; i < size; i++)
      {
        buffer[i] = buffer[i + offset];
      }
    }



    unsafe int ProcessMessages(byte* buffer, int length)
    {
      int processed = 0;
      int remainder = length;
      for (; ; )
      {
        if (remainder < sizeof(MessageHeader))
        {
          break;
        }
        MessageHeader* msgHeader = (MessageHeader*)(buffer + processed);
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
      m_mainForm.OnNewRequest(msg->m_requestID);
    }

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



    unsafe public void SendChangeRequest(int requestID, String status, uint doneQty, string note)
    {
      ChangeRequestMessage msg;
      msg.m_header.m_type = MessageType.mtChangeRequest;
      msg.m_header.m_size = sizeof(ChangeRequestMessage);
      msg.m_status = m_mainForm.GetStatusID(status);
      msg.m_doneQty = doneQty;
      msg.m_requestID = requestID;
      StringToByteArray(note, msg.m_bbsNote);
      byte* p = (byte*)&msg;
      for (int i = 0; i < sizeof(LoginRequest); i++)
      {
        m_sendBuffer[i] = p[i];
      }
      m_tcpClient.Send(m_sendBuffer, msg.m_header.m_size, SocketFlags.None);

    }


  }
}
