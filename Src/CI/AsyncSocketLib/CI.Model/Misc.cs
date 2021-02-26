using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace AsyncSocketLib.CI.Model
{
  public class Misc
  {
    readonly object m_thisLock = new object(); // public object CriticalSection => m_thisLock;
    readonly Socket m_tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    readonly byte[] m_recvBuffer = new byte[StateObject.BufferSize];
    readonly byte[] m_sendBuffer = new byte[StateObject.BufferSize];
    readonly HashSet<int> m_outstandingRequests = new HashSet<int>();

    class Request
    {
      public unsafe Request(MessageHeader* msgHeader) => m_msgHeader = msgHeader;
      public unsafe MessageHeader* m_msgHeader = null;
    }

    public static unsafe void StringToByteArray(string str, byte* buffer)
    {
      var byteArray = Encoding.ASCII.GetBytes(str);
      for (var i = 0; i < str.Length; i++)
        buffer[i] = byteArray[i];

      buffer[str.Length] = 0;
    }
  }
}