using System.Collections.Generic;

namespace LD58
{
    public static class ListExtensions
    {
        public static List<T> Clone<T>(this IReadOnlyList<T> list)
        {
            int fruit_count = list.Count;
            List<T> result = new(fruit_count);

            for (int fruit_index = 0; fruit_index < fruit_count; fruit_index++)
            {
                result.Add(list[fruit_index]);
            }

            return result;
        }
    }
}
