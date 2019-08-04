using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomsSharedCode.Networking
{
    static class Extensions
    {
        public static void Write(this Stream stream, Message message)
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(message.controller);
                writer.Write(message.beat);
                writer.Write(message.session);
                writer.Write(message.secret);
                writer.Write(Encoding.UTF8.GetBytes(message.body));
            }
        }
    }
}
