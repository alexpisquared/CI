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
    public partial class AsynchronousClient
    {
        readonly ManualResetEvent _connectDone = new(false), _sendingDone = new(false), _receiveDone = new(false);    // ManualResetEvent instances signal completion.  
        readonly DateTime _started;
        readonly object _theLock = new();
        string _report = "", _responseStrVer = ""; // The response from the remote device.  
        //RiskBaseMsg _rbm;
        int m_received;

        public AsynchronousClient() => _started = DateTime.Now;

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

        public async Task ConnectSendClose_formerStartClient(string uri, int port) => await ConnectSendClose(uri, port, "", "stringPoc");
        public async Task<string> ConnectSendClose(string uri, int port, string username, string job)
        {
            try
            {
                var ipHostInfo = Dns.GetHostEntry(uri);
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEP = new IPEndPoint(ipAddress, port);

                using var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket.  
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                var sw = Stopwatch.StartNew();
                if (!_connectDone.WaitOne(2500))
                {
                    return Report = $"  Failed to connect in allocated 2.5 sec\n";
                }

                Report = $"Trying to connect to \t{uri,44}:{port} ... succeeded in {sw.ElapsedMilliseconds} / 2500 \n";

                switch (job)
                {
                    case "stringPoc": sendOriginalStr(client, "This is a TEST <EOF>"); _sendingDone.WaitOne(); Receive<UnknownType>(client); break;
                    case "logInRmsC":
                        sendLoginRequestRmsC(client, username); _sendingDone.WaitOne(); Receive<UnknownType>(client);
                        await Task.Delay(1000 + 2500 + 500);
                        sendChngeRequestRmsC(client, 26859, RequestStatus.rsDone, 8, $"AAAAAAAAAzAAAAAAAAAzAAAAAAAAAzAAAAAAAAAzAAAAAAAAAzAAAAAAAAAzAAAAAAAAAz"); 
                        _sendingDone.WaitOne(); Receive<UnknownType>(client);
                        await Task.Delay(1000 + 2500 + 500);
                        break;
                    case "logInRisk": sendLoginRequestRisk(client, username); _sendingDone.WaitOne(); Receive<RiskBaseMsg>(client); break;
                    default: throw new Exception($"{job} is unknown");
                }

                sw = Stopwatch.StartNew();
                Report = "\n\n  Finally, waiting 1000 for the Response ■ ";
                _receiveDone.WaitOne(2000);
                Report = $"■ ■  in {sw.ElapsedMilliseconds} / 1000 response received: '{_responseStrVer}';  closing client socket.\n\n";

                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }

            return Report;
        }

        void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var client = (Socket)ar.AsyncState;   // Retrieve the socket from the state object.  
                client.EndConnect(ar);                // Complete the connection.  

                Report = ($"    ◄ Socket connected to \t\t{client.RemoteEndPoint,42} +++\n");

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
            Report = ($"    ◄ rcb ");
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

                        Report = $"{state.SB.Length,4}: '{state.SB}' got so far; maybe more coming...";

                        client?.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback<T>), state); // Get the rest of the data.  
                    }
                    m_received += bytesRead;
                }
                else
                {
                    Report = $"{state.SB.Length,4}: '{state.SB}' All the data has arrived";

                    if (state.SB.Length > 1)          // All the data has arrived; put it in response.  
                    {
                        _responseStrVer = $"{state.SB.Length,4}: '{state.SB}'";
                    }

                    _receiveDone.Set();               // Signal that all bytes have been received.  
                }

                //nogo: _rbm = FromByteArray<RiskBaseMsg>(state.buffer);
                if (m_received >= 12 && state?.Buffer != null)  //sizeof(RiskBaseMsg))
                {
                    fixed (byte* pSource = state.Buffer)
                    {
                        var msgHeader = (RiskBaseMsg*)pSource;
                        Report = $"\n       **RMB  seq sz tm gu: {(*msgHeader).m_seq,16}{(*msgHeader).m_size,16}{(*msgHeader).m_time,20}{(*msgHeader).iGuidID,16}{(*msgHeader).m_type,-32}·\n";
                        if (m_received >= msgHeader->m_size)
                        {
                            var bytesProcessed = ProcessMessages(pSource, m_received);
                            var remainder = m_received - bytesProcessed;
                            if (remainder > 0)
                            {
                                Report = $" \t {remainder} > 0   =>   data moved!";
                                BinaryHelper.MoveData(state.Buffer, bytesProcessed, remainder);
                            }
                            m_received -= bytesProcessed;
                        }
                    }
                }

                _responseStrVer = state?.SB.ToString() ?? "Emtpy as null can be";
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
            LoginRequest lr;
            lr.m_header.m_size = sizeof(LoginRequest);
            lr.m_header.m_type = MessageType.mtLogin;
            lr.m_password[0] = 0;
            username.ToByteArray(lr.m_userName);

            var ptr = (byte*)&lr;
            var byteData = new byte[StateObject.BufferSize];
            for (var i = 0; i < sizeof(LoginRequest); i++) byteData[i] = ptr[i];
            for (var i = 20; i < sizeof(LoginRequest); i++) byteData[i] = 205; //mimic cpp: 

            Report = $"  ►{(DateTime.Now - _started):s\\.fff} sending \t{nameof(LoginRequest),-26} \t\t total: {sizeof(LoginRequest)}\n";             //

            //for (var i = 0; i < sizeof(LoginRequest); i++) Debug.WriteLine($"  {i,4} {(int)byteData[i],4}"); Debug.WriteLine($"-- total: {sizeof(LoginRequest)} for log-in request.");

            send(client, byteData, sizeof(LoginRequest)); // client.BeginSend(byteData, 0, sizeof(LoginRequest), 0, new AsyncCallback(SendCallback), client);   
        }
        unsafe void sendChngeRequestRmsC(Socket client, int requestID, RequestStatus status, uint doneQty, string note)
        {
            const int sz = 153;
            ChangeRequest crm;
            crm.m_messageHeader.m_size = 153;
            crm.m_messageHeader.m_type = MessageType.mtChangeRequest;
            crm.m_data.m_orderID = requestID;
            crm.m_data.m_type = (int)MessageType.mtResponse;
            crm.m_data.m_parentID = 0;
            crm.m_data.m_status = (int)status;
            crm.m_data.m_lastShares = 8;// (int)doneQty;
            crm.m_data.m_price = 9;

            //crm.m_time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            crm.m_data.m_int64Time = 132597104686937912;

            //crm.m_data.m_time.dwLowDateTime = 991704888;
            //crm.m_data.m_time.dwHighDateTime = 30872669;

            note.ToByteArray(crm.m_data.m_bbsNote);

            var ptr = (byte*)&crm;
            var byteData = new byte[StateObject.BufferSize];
            for (var i = 0; i < sz; i++) byteData[i] = ptr[i];

            Report = $"  ►{(DateTime.Now - _started):s\\.fff} sending \t{nameof(ChangeRequestMessage_RMS)} ...  -- total: {sz}\n";

            for (var i = 0; i < sz; i++) Debug.WriteLine($"  {i,4} {(byteData[i] == 0 ? "" : $"{(int)byteData[i],4}")}"); Debug.WriteLine($"-- total: { sz} for change request.");

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
                Report = ($"    ◄ scb  Sent{bytesSent,4}  bytes to server.\n");
                _sendingDone.Set();                 // Signal that all bytes have been sent.  
            }
            catch (Exception ex) { Report = $" ■ ■ ■ {ex}\n\n"; }
        }
    }
}