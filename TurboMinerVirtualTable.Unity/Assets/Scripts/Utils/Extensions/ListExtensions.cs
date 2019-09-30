using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Utils.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int listCount = list.Count;
            while (listCount > 1)
            {
                listCount--;
                int randomIndex = RngHelper.GetRandom(0, listCount + 1);
                T value = list[randomIndex];
                list[randomIndex] = list[listCount];
                list[listCount] = value;
            }
        }

        public static List<List<T>> ChunkBy<T>(this IList<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
