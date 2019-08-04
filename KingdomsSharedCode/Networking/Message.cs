using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomsSharedCode.Networking
{
    public class Message
    {
        public byte controller;
        public byte beat;
        public uint session;
        public uint secret;
        public string body = string.Empty;

        public Message(byte[] buffer, int bytesRead) : this(new BinaryReader(new MemoryStream(buffer), Encoding.UTF8)) {}

        public Message(BinaryReader reader)
        {
            controller = reader.ReadByte();
            beat = reader.ReadByte();
            session = reader.ReadUInt32();
            secret = reader.ReadUInt32();
            body = reader.ReadString();
        }

        public Message() { }

        public override string ToString()
        {
            return string.Format("[{0}  {1}  {2}  {3}]     {4}",
                controller.ToString("X2"),
                beat.ToString("X2"),
                session.ToString("X8"),
                secret.ToString("X8"),
                body
            );
        }

        public void Check()
        {
            //throw new BrokenMessageException();
        }
    }
}
