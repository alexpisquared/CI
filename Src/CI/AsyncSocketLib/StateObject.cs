using System.Net.Sockets;
using System.Text;

namespace AsyncSocketLib
{
  public class StateObject // State object for receiving data from remote device.  
  {
    public Socket? workSocket = null;               // Client socket.  
    public const int BufferSize = 1024;             // Size of receive buffer.  
    public byte[] buffer = new byte[BufferSize];    // Receive buffer.  
    public StringBuilder sb = new StringBuilder();  // Received data string.  
  }
}
