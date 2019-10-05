using System.Collections.Generic;

namespace Assets.Scripts.Elements
{
    public static class SortingLayers
    {
        public const string Pawns = "Pawns";
        public const string Tiles = "Tiles";
        public const string Passages = "Passages";
        public const string Corridors = "Corridors";
        public const string Board = "Board";

        public static Dictionary<string, float> LayerHeights = new Dictionary<string, float>
        {
            { Pawns, 6.0f },
            { Tiles, 5.0f },
            { Passages, 4.0f },
            { Corridors, 3.0f }
        };
    }
}
