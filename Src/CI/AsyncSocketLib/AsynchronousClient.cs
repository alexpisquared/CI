using AsyncSocketLib.CI.Model;
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
        readonly ManualResetEvent _connectDone = new ManualResetEvent(false); // ManualResetEvent instances signal completion.  
        readonly ManualResetEvent _sendingDone = new ManualResetEvent(false);
        readonly ManualResetEvent _receiveDone = new ManualResetEvent(false);
        string _report = "", _response = ""; // The response from the remote device.  
        RiskBaseMsg _rbm;
        private int m_received;

        public string Report { get { try { return _report; } finally { _report = ""; } } }

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
                _report += $"Trying to connect to \t{uri}:{port} ...\n";
                _connectDone.WaitOne();

                var msg = "This is a TEST <EOF>";
                _report += $"  sending \t{(string.IsNullOrEmpty(username) ? msg : username)} ...\n";

                if (string.IsNullOrEmpty(username))
                    sendOriginalStr(client, msg);
                else
                    sendLoginRequest154(client, username);

                _sendingDone.WaitOne();

                Receive(client); // Receive the response from the remote device.  
                _report += "  waiting for the Response ...\n";
                _receiveDone.WaitOne(1000); // wait just for a sec

                _report += $"  response received: '{_response}'\n\n";

                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception e) { _report += $" ■ ■ ■ {e}\n\n"; }
        }

        void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var client = (Socket)ar.AsyncState;   // Retrieve the socket from the state object.  
                client.EndConnect(ar);                // Complete the connection.  

                _report += ($"    <= Socket connected to \t{client.RemoteEndPoint} +++\n");

                _connectDone.Set();                   // Signal that the connection has been made.  
            }
            catch (Exception e) { _report += $" ■ ■ ■ {e}\n\n"; }
        }
        void Receive(Socket client)
        {
            try
            {
                var state = new StateObject { workSocket = client };

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e) { _report += $" ■ ■ ■ {e}\n\n"; }
        }
       unsafe void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var state = (StateObject)ar.AsyncState;        // Retrieve the state object and the client socket from the asynchronous state object.  
                var client = state.workSocket;

                var bytesRead = client?.EndReceive(ar) ?? 0; // Read data from the remote device.  
                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));          // There might be more data, so store the data received so far.  

                    _report += ($"    <= '{state.sb}' got so far ...maybe more coming...\n");

                    client?.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state); // Get the rest of the data.  
                }
                else
                {
                    if (state.sb.Length > 1)          // All the data has arrived; put it in response.  
                    {
                        _response = state.sb.ToString();
                    }

                    _receiveDone.Set();               // Signal that all bytes have been received.  
                }

                m_received += bytesRead;
                //_rbm = FromByteArray<RiskBaseMsg>(state.buffer);
                if (m_received >= 12 && state?.buffer!= null)//sizeof(RiskBaseMsg))
                {
                    fixed (byte* pSource = state.buffer )
                    {
                        RiskBaseMsg* msgHeader = (RiskBaseMsg*)pSource;
                        if (m_received >= msgHeader->m_size)
                        {
                            int bytesProcessed = ProcessMessages(pSource, m_received);
                            int remainder = m_received - bytesProcessed;
                            if (remainder > 0)
                            {
                                MoveData(state.buffer, bytesProcessed, remainder);
                            }
                            m_received -= bytesProcessed;
                        }
                    }
                }

                _response = state?.sb.ToString() ?? "Emtpy as null can be";
            }
            catch (Exception e) { _report += $" ■ ■ ■ {e}\n\n"; }
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


        public byte[]? ToByteArray<T>(T? obj)
        {
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            using var ms = new MemoryStream(data);
            var bf = new BinaryFormatter();
            var obj = bf.Deserialize(ms);
            return (T)obj;
        }
        void sendOriginalStr(Socket client, string stringData)
        {
            var byteData = Encoding.ASCII.GetBytes(stringData);
            send(client, byteData, byteData.Length);
        }
        unsafe void sendLoginRequest152(Socket client, string username)
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
        unsafe void sendLoginRequest154(Socket client, string username)
        {
            var byteData = new byte[StateObject.BufferSize];

            RiskBaseMsg lr;
            lr.m_type = RiskMsgType.RISK_MSG_LOGON;
            lr.m_seq = 0;
            lr.m_size = 32;
            lr.m_time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            lr.iGuidID = 101;

            var ptr = (byte*)&lr;
            for (var i = 0; i < sizeof(RiskBaseMsg); i++) byteData[i] = ptr[i];            //mimic cpp: for (var i = 20; i < sizeof(RiskBaeMsg); i++) byteData[i] = 205;

            Debug.WriteLine($"-- size C# / C++: {sizeof(RiskBaseMsg)} != {lr.m_size}:"); // for (var i = 0; i < sizeof(RiskBaeMsg); i++) Debug.WriteLine($"  {i,4} {(int)byteData[i],4}"); Debug.WriteLine($"-- total: {sizeof(RiskBaeMsg)}:");

            send(client, byteData, lr.m_size);
        }

        void send(Socket client, byte[] bytes, int len) => client.BeginSend(bytes, 0, len, 0, new AsyncCallback(sendCallback), client); // Begin sending the data to the remote device.      
        void sendCallback(IAsyncResult ar)
        {
            try
            {
                var client = (Socket)ar.AsyncState; // Retrieve the socket from the state object.  
                var bytesSent = client.EndSend(ar); // Complete sending the data to the remote device.  
                _report += ($"    <= Sent  {bytesSent}  bytes to server.\n");
                _sendingDone.Set();                 // Signal that all bytes have been sent.  
            }
            catch (Exception e) { _report += $" ■ ■ ■ {e}\n\n"; }
        }
    }
}