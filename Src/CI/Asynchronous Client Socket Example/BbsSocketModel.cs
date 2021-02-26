using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Asynchronous_Client_Socket_Example
{
  class BbsSocketModel { }

  public partial class AsynchronousClient
  {
    readonly object m_thisLock = new object(); // public object CriticalSection => m_thisLock;
    readonly Socket m_tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    readonly byte[] m_recvBuffer = new byte[BufferSize];
    readonly byte[] m_sendBuffer = new byte[BufferSize];
    readonly HashSet<int> m_outstandingRequests = new HashSet<int>();
    const int BufferSize = 1000;
    readonly int m_received = 0;
    static uint m_seqNo = 0;

    class Request
    {
      public unsafe Request(MessageHeader* msgHeader) => m_msgHeader = msgHeader;
      public unsafe MessageHeader* m_msgHeader = null;
    }

    static unsafe void StringToByteArray(string str, byte* buffer)
    {
      var byteArray = Encoding.ASCII.GetBytes(str);
      for (var i = 0; i < str.Length; i++)
        buffer[i] = byteArray[i];

      buffer[str.Length] = 0;
    }
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
    rsDone = 7,
    rsCancelRequested = 8 // new 2021
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
    public fixed byte m_eof[5]; // = "<EOF>";
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
    [FieldOffset(sizeof(int) * 7 + sizeof(double) + sizeof(char) * 101)] public long  /*long long*/ m_int64Time;
  };
#endif
}
