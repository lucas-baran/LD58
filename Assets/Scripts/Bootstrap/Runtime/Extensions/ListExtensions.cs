using System.Collections.Generic;

namespace LucasBaran.Bootstrap
{
    public static class ListExtensions
    {
        public static bool Contains<T>(this IReadOnlyList<T> list, T item, IEqualityComparer<T> comparer = null)
        {
            bool contains = false;
            comparer ??= EqualityComparer<T>.Default;

            for (int scenario_index = 0; !contains && scenario_index < list.Count; scenario_index++)
            {
                contains = comparer.Equals(item, list[scenario_index]);
            }

            return contains;
        }
    }
}
