using System;
using System.Collections.Generic;
using System.Text;

namespace KingdomsSharedCode.Networking
{
    public enum Controller : byte
    {
        // RELAY
        RELAY_HOST_SESSION  =   1,
        RELAY_JOIN_SESSION  =   2,
        RELAY_LEAVE_SESSION =   3,
        RELAY_HEARTBEAT     =   4,

        // CLIENT
        SESSION_INFO        =   5,      // Contains session info and starting beat
        BROADCAST           =   6,      // Debug message to be broadcasted to every client of the same session
        WAIT                =   7,      // Client must pause its internal clock to wait for other clients
        GO                  =   8,      // Client can resume its internal clock
        DESYNCHRONIZED      =   9,      // Game state is desynchronized between clients, simulation should be interrupted.
        CHAT                =   10      // Chat message. Similar to Broadcast
    }
}
