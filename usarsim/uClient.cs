namespace usarsim
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class uClient
    {
        public int SendTimeout = 5000;

        public string HostName { get; private set; }
        public int HostPort { get; private set; }

        public bool IsConnected { get { return _tcpClient != null && _tcpClient.Connected; } }
        public bool IsConnecting { get; private set; }

        public event Action<string> ErrorOccurred;
        public event Action<string> LogMessage;

        private TcpClient _tcpClient;

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
            _tcpClient.SendTimeout = SendTimeout;
            _tcpClient.Connect(HostName, HostPort);

            IsConnecting = false;

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

        public void SpawnRobot()
        {
            string command = uCommsProtocol.INIT();
            Send(command);
        }

        public void Drive(float left, float right)
        {
            string command = uCommsProtocol.DRIVE(left, right);
            Send(command);
        }

        public void Send(string message)
        {
            if (!IsConnected)
            {
                ErrorOccurred.Raise("not connected, so cannot send message");
                return;
            }

            var buffer = Encoding.ASCII.GetBytes(message);
            _tcpClient.Client.Send(buffer);

            LogMessage.Raise("sent message: " + message);
        }
    }
}
