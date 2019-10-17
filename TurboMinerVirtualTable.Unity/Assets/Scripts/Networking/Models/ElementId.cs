using System;

namespace Assets.Scripts.Networking.Models
{
    [Serializable]
    public class ElementId
    {
        public int Id;

        public ElementId(int id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Needed for JsonUtility, because it cannot serialize array directly
    /// </summary>
    [Serializable]
    public class ElementIdArray
    {
        public ElementId[] Array;

        public ElementIdArray(int size)
        {
            Array = new ElementId[size];
        }
    }
}
