namespace usarsim
{
    using System;

    public static class uCommsProtocol
    {
        public static string INIT()
        {
            string msg = "INIT";
            msg = addParam(msg, "ClassName", "USARBot.P3AT");
            msg = addParam(msg, "Start", "RobotStart2");
            //msg = addParam(msg, "Start", "AutoPlayerStart_0");
            //msg = addParam(msg, "Location", "4.5,1.9,1.8");
            msg += "\r\n";

            return msg;
        }

        public static string DRIVE(float left, float right)
        {
            string msg = "DRIVE";
            msg = addParam(msg, "Left", left);
            msg = addParam(msg, "Right", right);
            msg += "\r\n";

            return msg;
        }

        private static string addParam(string message, string paramName, object paramValue)
        {
            return message + " {" + paramName + " " + paramValue.ToString() + "}";
        }

    }
}
