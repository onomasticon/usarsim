namespace usarsim.remote
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using usarsim;
    
    public partial class MainWindow : Window
    {
        private uRobotClient _robotClient;
        private uImageClient _imageClient;

        public MainWindow()
        {
            InitializeComponent();

            _robotClient = new uRobotClient();
            _robotClient.ErrorOccurred += log;
            _robotClient.LogMessage += log;

            _imageClient = new uImageClient();
            _imageClient.ErrorOccurred += log;
            _imageClient.LogMessage += log;

            button_connect.Click += (o, e) =>
            {
                toggleConnection();
            };
            button_spawn.Click += (o, e) =>
            {
                _robotClient.SpawnRobot();

                _imageClient.HostPort = 5003;
                _imageClient.Connect();
            };
            button_process.Click += (o, e) =>
            {
                _imageClient.Send("OK\r\n");
                _imageClient.Receive();
                _imageClient.Process();
            };

            this.KeyDown += (o, e) =>
            {
                var power = 1f;

                if (e.Key == Key.Up) _robotClient.Drive(power, power);
                else if (e.Key == Key.Left) _robotClient.Drive(-power, power);
                else if (e.Key == Key.Right) _robotClient.Drive(power, -power);
                else if (e.Key == Key.Down) _robotClient.Drive(-power, -power);
            };
        }

        private void toggleConnection()
        {
            if (_robotClient.IsConnected)
            {
                _robotClient.Disconnect();
                _imageClient.Disconnect();
                log("successfully disconnected");
            }
            else
            {
                var host = tx_hostname.Text;
                var port = int.Parse(tx_hostport.Text);
                var msg = "connecting to ";
                msg += "host: " + host + " ";
                msg += "port: " + port + "...";
                log(msg);

                _robotClient.HostName = host;
                _robotClient.HostPort = port;
                _robotClient.Connect();

                log("successfully connected!");
            }
            
            button_connect.Content = _robotClient.IsConnected ? "Disconnect" : "Connect";
        }

        private void log(string message)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                {
                    message = message.Replace("\r\n", "");
                    tb_log_message.Text += "> " + message + "\n";
                }), null);
        }
    }
}
