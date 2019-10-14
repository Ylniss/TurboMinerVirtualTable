namespace Assets.Scripts.Networking
{
    public static class MessageCommands
    {
        public static class Server
        {
            public const string Who = "SWHO";
            public const string Connected = "SCNN";
            public const string Start = "SSTR";
            public const string WidthSettings = "SSETW";
            public const string HeightSettings = "SSETH";
            public const string TilesConfigName = "STCFG";
            public const string CorridorsConfigName = "SCCFG";
        }

        public static class Client
        {
            public const string Who = "CWHO";
            public const string Connected = "CCNN";
            public const string Start = "CSTR";
            public const string Settings = "CSET";
            public const string WidthSettings = "CSETW";
            public const string HeightSettings = "CSETH";
            public const string TilesConfigName = "CTCFG";
            public const string CorridorsConfigName = "CCCFG";
        }
    }
}
