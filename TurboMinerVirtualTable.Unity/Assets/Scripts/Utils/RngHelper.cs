using System;
using System.Security.Cryptography;

namespace Assets.Scripts.Utils
{
    public static class RngHelper
    {
        private static readonly RandomNumberGenerator generator;

        static RngHelper()
        {
            generator = RandomNumberGenerator.Create();
        }

        public static int GetRandom(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                generator.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
        }
    }
}
