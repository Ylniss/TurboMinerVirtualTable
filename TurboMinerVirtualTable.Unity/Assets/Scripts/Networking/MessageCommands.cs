using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Networking
{
    public static class MessageCommands
    {
        public static class Server
        {
            public const string Who = "SWHO";
            public const string Connected = "SCNN";
            public const string Start = "SSTR";
            public const string Settings = "SSET";
        }

        public static class Client
        {
            public const string Who = "CWHO";
            public const string Connected = "CCNN";
            public const string Start = "CSTR";
            public const string Settings = "CSET";
        }
    }
}
