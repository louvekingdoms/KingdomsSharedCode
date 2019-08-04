﻿using System;
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
        SESSION_INFO        =   5,
        BROADCAST           =   6
    }
}
