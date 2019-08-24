using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KingdomsSharedCode.Networking
{
    static class Extensions
    {
        public static NetworkStream NewStream(this Socket socket)
        {
            return new NetworkStream(socket);
        }

        public static byte[] ReadMessageData(this NetworkStream netStream)
        {
            var lengthBuffer = new byte[4]; // uint is 4 bytes
            netStream.Read(lengthBuffer, 0, lengthBuffer.Length);
            var length = BitConverter.ToUInt32(lengthBuffer, 0);
            var msgBuffer = new byte[length];

            // Emptying network buffer from data
            var totalDataRead = 0;
            List<byte[]> buffers = new List<byte[]>();
            while (totalDataRead < length)
            {
                byte[] buffer = new byte[1024];
                var dataRead = netStream.Read(buffer, 0, (int)length);
                totalDataRead += dataRead;

                byte[] shrankArray = new byte[dataRead];
                Array.Copy(buffer, shrankArray, shrankArray.Length);

                buffers.Add(shrankArray);
            }

            // Concatenate data
            var index = 0;
            foreach (var buffer in buffers)
            {
                buffer.CopyTo(msgBuffer, index);
                index = buffer.Length;
            }
            return msgBuffer;
        }

        public static void Write(this NetworkStream stream, byte[] data)
        {
            lock (stream)
            {
                // Writing message size
                stream.Write(data, 0, data.Length);
            }
        }

        public static void Write(this NetworkStream stream, Message message)
        {
            stream.Write(message.Serialize());
        }
    }
}
