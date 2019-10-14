using System;

namespace Assets.Scripts.Settings.Models
{
    [Serializable]
    public class MapSize
    {
        public int Width;
        public int Height;

        public MapSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
