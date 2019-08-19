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

        public static void Write(this NetworkStream stream, Message message)
        {
            // Can throw SocketException and IOException
            if (!stream.CanWrite)
            {
                throw new Exception("Expected to be able to write into the network stream, but could not (canWrite is false)");
            }
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                writer.Write(message.controller);
                writer.Write(message.beat);
                writer.Write(message.sumAtBeat);
                writer.Write(message.session);
                writer.Write(message.secret);
                writer.Write(message.body);
            }
        }
    }
}
