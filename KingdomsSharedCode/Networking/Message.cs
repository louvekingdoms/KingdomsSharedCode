using System;
using System.Collections.Generic;
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
        public string body;

        public void Check()
        {
            //throw new BrokenMessageException();
        }
    }
}
