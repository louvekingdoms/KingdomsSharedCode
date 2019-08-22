using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KingdomsSharedCode.Networking
{
    public class Message
    {
        public byte controller;
        public ushort beat;
        public byte[] sumAtBeat = new byte[16];// MD5 hash size is 32 bytes
        public uint session;
        public uint secret;
        public string body = string.Empty;

        //public Message(byte[] buffer, int bytesRead) : this(new BinaryReader(new MemoryStream(buffer), Encoding.UTF8)) {}

        public Message(BinaryReader reader)
        {
            controller = reader.ReadByte();
            beat = reader.ReadUInt16();
            sumAtBeat = reader.ReadBytes(sumAtBeat.Length);
            session = reader.ReadUInt32();
            secret = reader.ReadUInt32();
            body = reader.ReadString();
        }

        public Message() { }

        public void Write(NetworkStream stream)
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                var mes = this;
                writer.Write(mes.controller);
                writer.Write(mes.beat);
                writer.Write(mes.sumAtBeat);
                writer.Write(mes.session);
                writer.Write(mes.secret);
                writer.Write(mes.body);
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}  BEAT:{1}  SES:{2}  SEC:{3}  SUM:{4}] [{5}]",
                ((Controller)controller).ToString(),
                beat.ToString("X4"),
                session.ToString("X8"),
                secret.ToString("X8"),
                string.Join(" ", sumAtBeat),
                body
            );
        }

        public void Check()
        {
            //throw new BrokenMessageException();
        }
    }
}
