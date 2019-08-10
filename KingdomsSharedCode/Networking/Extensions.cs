using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static KingdomsSharedCode.Generic.Logger;

namespace KingdomsSharedCode.Networking
{
    static class Extensions
    {
        public static NetworkStream NewStream(this Socket socket)
        {
            return new NetworkStream(socket);
        }

        public static void Write(this NetworkStream stream, Message message)
        {
            if (!stream.CanWrite)
            {
                Console.Write("Stream cannot be written to, skipping " + message);
                return;
            }

            Trace("WRITING: " + message);

            try
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    writer.Write(message.controller);
                    writer.Write(message.beat);
                    writer.Write(message.session);
                    writer.Write(message.secret);
                    writer.Write(message.body);
                }
            }
            catch (SocketException e)
            {
                Error("Socket error while communicating, closing the stream");
                Error(e.ToString());
                stream.Close();
            }
            catch (IOException e)
            {
                Error("IO error while communicating, closing the stream");
                Error(e.ToString());
                stream.Close();
            }
        }
    }
}
