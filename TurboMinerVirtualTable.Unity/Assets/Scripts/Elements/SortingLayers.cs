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
            { Pawns, 200.0f },
            { Tiles, 105.0f },
            { Passages, 55.0f },
            { Corridors, 5.0f }
        };
    }
}
