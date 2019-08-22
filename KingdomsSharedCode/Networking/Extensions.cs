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
            message.Write(stream);
        }
    }
}
