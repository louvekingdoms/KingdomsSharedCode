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
        public uint size; 
        public byte controller;
        public ushort beat;
        public uint session;
        public ushort owner;
        public uint secret;
        public string body = string.Empty;


        public Message(byte[] data){
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(ms, Encoding.UTF8))
                {
                    size = (uint)data.Length;
                    controller = reader.ReadByte();
                    beat = reader.ReadUInt16();
                    session = reader.ReadUInt32();
                    owner = reader.ReadUInt16();
                    secret = reader.ReadUInt32();
                    body = reader.ReadString();
                }
            }
        }

        public Message() { }

        public byte[] Serialize()
        {
            using (var memoryWriter = new BinaryWriter(new MemoryStream(), Encoding.UTF8))
            {
                var mes = this;
                memoryWriter.Write(mes.controller);
                memoryWriter.Write(mes.beat);
                memoryWriter.Write(mes.session);
                memoryWriter.Write(mes.owner);
                memoryWriter.Write(mes.secret);
                memoryWriter.Write(mes.body);

                mes.size = (uint)memoryWriter.BaseStream.Length;

                byte[] data = new byte[mes.size+4]; // size uint is 4 bytes heavy
                BitConverter.GetBytes(mes.size).CopyTo(data, 0);
                ((MemoryStream)memoryWriter.BaseStream).ToArray().CopyTo(data, 4);

                return data;
            }
        }

        public override string ToString()
        {
            return string.Format("(Player.{6}) [{0}:{5} BEAT:{1} SES:{2} SEC:{3}] [{4}]",
                ((Controller)controller).ToString(),
                beat.ToString("X4"),
                session.ToString("X8"),
                secret.ToString("X8"),
                body,
                size.ToString(),
                owner.ToString("00000")
            );
        }

        public void Check()
        {
            //throw new BrokenMessageException();
        }
    }
}
