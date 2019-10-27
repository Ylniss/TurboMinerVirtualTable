using System;
using UnityEngine;

namespace Assets.Scripts.Networking.Models
{
    [Serializable]
    public class ElementPosition
    {
        public int Id;
        public Vector2 Position;

        public ElementPosition(int id, Vector2 position)
        {
            Id = id;
            Position = position;
        }
    }

    /// <summary>
    /// Needed for JsonUtility, because it cannot serialize array directly
    /// </summary>
    [Serializable]
    public class ElementPositionArray
    {
        public ElementPosition[] Array;

        public ElementPositionArray(int size)
        {
            Array = new ElementPosition[size];
        }
    }
}
