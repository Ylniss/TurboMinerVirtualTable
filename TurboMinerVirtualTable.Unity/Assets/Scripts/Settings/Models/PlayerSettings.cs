using System;

namespace Assets.Scripts.Settings.Models
{
    [Serializable]
    public class PlayerSettings
    {
        public string Name;
        public string Color;

        public PlayerSettings(string name, string color)
        {
            Name = name;
            Color = color;
        }
    }


    /// <summary>
    /// Needed for JsonUtility, because it cannot serialize array directly
    /// </summary>
    [Serializable]
    public class PlayerSettingsArray
    {
        public PlayerSettings[] Array;
    }
}
