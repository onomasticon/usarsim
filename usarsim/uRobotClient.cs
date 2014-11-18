namespace usarsim
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class uRobotClient : uClient
    {
        public bool IsDriving { get; protected set; }

        public void SpawnRobot()
        {
            string command = uCommsProtocol.INIT();
            Send(command);
        }

        public void Drive(float left, float right)
        {
            string command = uCommsProtocol.DRIVE(left, right);
            Send(command);
            IsDriving = true;
        }
    }
}
