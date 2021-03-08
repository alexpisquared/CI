using System.Net.Sockets;
using System.Text;

namespace AsyncSocketLib
{
  public class StateObject // State object for receiving data from remote device.  
  {
    public Socket? WorkSocket = null;               // Client socket.  
    public const int BufferSize = 1024;             // Size of receive buffer.  
    public byte[] Buffer = new byte[BufferSize];    // Receive buffer.  
    public StringBuilder SB = new();                // Received data string.  
  }
}
