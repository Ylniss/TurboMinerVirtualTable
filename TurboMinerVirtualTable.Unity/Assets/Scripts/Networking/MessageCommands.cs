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
            public const string ElementPosition = "SEPOS";
            public const string ElementStopDrag = "SEDRG";
            public const string ElementLayer = "SELAY";
            public const string ElementTurn = "SETUR";
            public const string ElementRotate = "SEROT";
            public const string RollDice = "SDICE";
        }

        public static class Client
        {
            public const string Who = "CWHO";
            public const string Connected = "CCNN";
            public const string Start = "CSTR";
            public const string WidthSettings = "CSETW";
            public const string HeightSettings = "CSETH";
            public const string TilesConfigName = "CTCFG";
            public const string CorridorsConfigName = "CCCFG";
            public const string ElementPosition = "CEPOS";
            public const string ElementStopDrag = "CEDRG";
            public const string ElementLayer = "CELAY";
            public const string ElementTurn = "CETUR";
            public const string ElementRotate = "CEROT";
            public const string RollDice = "CDICE";
        }
    }
}
