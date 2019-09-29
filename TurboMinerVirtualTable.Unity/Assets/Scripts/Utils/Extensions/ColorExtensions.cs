using UnityEngine;

namespace Assets.Scripts.Utils.Extensions
{
    public static class ColorExtensions
    {
        public static string ToColorString(this Color c)
        {
            if (c == Color.white) return "white";
            if (c == Color.black) return "black";
            if (c == Color.red) return "red";
            if (c == Color.green) return "green";
            if (c == Color.blue) return "blue";
            if (c == Color.yellow) return "yellow";

            return string.Empty;
        }

        public static Color ToColor(this Color c, string color)
        {
            if (color == "white") return Color.white;
            if (color == "black") return Color.black;
            if (color == "red") return Color.red;
            if (color == "green") return Color.green;
            if (color == "blue") return Color.blue;
            if (color == "yellow") return Color.yellow;

            return c;
        }

    }
}
