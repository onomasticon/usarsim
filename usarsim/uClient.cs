namespace usarsim
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class uClient
    {
        public int SendTimeout = 1;

        public string HostName { get; set; }
        public int HostPort { get; set; }

        public bool IsConnected { get { return _tcpClient != null && _tcpClient.Connected; } }
        public bool IsConnecting { get; protected set; }

        public event Action<string> ErrorOccurred;
        public event Action<string> LogMessage;

        protected TcpClient _tcpClient;
        protected NetworkStream _stream;

        protected Queue<byte[]> _sendQueue = new Queue<byte[]>();
        protected Queue<byte[]> _receiveQueue = new Queue<byte[]>();

        public uClient() : this("localhost", 3000)
        {
        }

        public uClient(string hostName, int port)
        {
            HostName = hostName;
            HostPort = port;
        }

        public void Connect()
        {
            if (_tcpClient != null)
            {
                Disconnect();
            }

            IsConnecting = true;

            _tcpClient = new TcpClient();
            _tcpClient.Connect(HostName, HostPort);

            _tcpClient.SendTimeout = SendTimeout;
            _tcpClient.Client.SendTimeout = SendTimeout;
            _stream = _tcpClient.GetStream();

            IsConnecting = false;
            LogMessage.Raise("connected to " + HostName + " : " + HostPort);

            // TODO: this is currently blocking. do we need to make it async?
        }

        public void Disconnect()
        {
            if (_tcpClient == null)
            {
                ErrorOccurred.Raise("not connected, so cannot disconnect");
                return;
            }

            _tcpClient.Close();
            _tcpClient = null;
        }

        public void Receive()
        {
            var readBuffer = readStreamContents();
            _receiveQueue.Enqueue(readBuffer);
        }

        public void Send(string message)
        {
            if (!IsConnected)
            {
                ErrorOccurred.Raise("error: not connected, so cannot send message");
                return;
            }

            var buffer = Encoding.ASCII.GetBytes(message);
            _tcpClient.Client.Send(buffer);

            LogMessage.Raise("sent message: " + message);
        }

        protected byte[] readStreamContents()
        {
            List<byte> buffer = new List<byte>();
            int readBuffer = 0;

            while (_stream.DataAvailable)
            {
                readBuffer = _stream.ReadByte();

                if (readBuffer != -1)
                {
                    buffer.Add((byte)readBuffer);
                }
            }
            return buffer.ToArray();
        }
    }
}
