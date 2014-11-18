namespace usarsim
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class uImageClient : uClient
    {
        public void Process()
        {
            if (_receiveQueue.Count > 0)
            {
                var rawData = _receiveQueue.Dequeue();

                if (rawData.Length > 0)
                {
                    var dataLength = rawData.Length-5;
                    var imageData = new byte[dataLength];

                    Array.Copy(rawData, 5, imageData, 0, dataLength);

                    using (var image = Image.FromStream(new MemoryStream(imageData), false, false))
                    {
                        image.Save("output.jpg", ImageFormat.Jpeg);
                    }
                }
            }
        }
    }
}
