using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
