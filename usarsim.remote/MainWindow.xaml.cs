namespace usarsim.remote
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using usarsim;
    
    public partial class MainWindow : Window
    {
        private uClient _client;

        public MainWindow()
        {
            InitializeComponent();

            _client = new uClient();
            _client.ErrorOccurred += errorMsg =>
            {
                string msg = "error: " + errorMsg;
                log(msg);
            };
            _client.LogMessage += msg =>
            {
                log(msg);
            };

            button_connect.Click += (o, e) =>
            {
                toggleConnection();
            };
            button_spawn.Click += (o, e) =>
            {
                _client.SpawnRobot();
            };

            this.KeyDown += (o, e) =>
            {
                var power = 0.5f;

                if (e.Key == Key.Up) _client.Drive(power, power);
                else if (e.Key == Key.Left) _client.Drive(-power, power);
                else if (e.Key == Key.Right) _client.Drive(power, -power);
                else if (e.Key == Key.Down) _client.Drive(-power, -power);
            };
        }

        private void toggleConnection()
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
                log("successfully disconnected");
            }
            else
            {
                var msg = "connecting to ";
                msg += "host: " + _client.HostName + " ";
                msg += "port: " + _client.HostPort + "...";
                log(msg);

                _client.Connect();

                log("successfully connected!");
            }
            
            button_connect.Content = _client.IsConnected ? "Disconnect" : "Connect";
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
