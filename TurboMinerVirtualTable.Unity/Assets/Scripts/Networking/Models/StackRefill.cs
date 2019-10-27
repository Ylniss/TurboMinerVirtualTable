using System;

namespace Assets.Scripts.Networking.Models
{
    [Serializable]
    public class StackRefill
    {
        public int Id;
        public string[] RefillArray;

        public StackRefill(int id, string[] refillArray)
        {
            Id = id;
            RefillArray = refillArray;
        }
    }
}
