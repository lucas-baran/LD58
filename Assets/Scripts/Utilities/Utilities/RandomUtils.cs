using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD58
{
    public static class RandomUtils
    {
        /// <summary>
        /// Selects an item in a successive manner, each candidate item has the same chance of being selected.<br/>
        /// This function can be called multiple times in a row, it will update the necessary variables to work as expected.<br/>
        /// It can be used to select a random item that satisfies a predicate in any type of collection (e.g. <see cref="GetRandomItem"/>) or algorithm.
        /// </summary>
        /// <param name="random_value">A value starting inside [0,1]<br />Giving the same starting value will produce the same result.</param>
        /// <param name="selection_index">The index of selection, this ensures that the chances to select each item are the same.</param>
        public static void SelectRandomItem<TItem>(ref float random_value, ref int selection_index, ref bool has_first_item, ref TItem selected_item, TItem candidate_item)
        {
            if (!has_first_item)
            {
                has_first_item = true;
                selected_item = candidate_item;

                return;
            }

            random_value = Mathf.Abs(random_value) % 1f;
            float chance_of_keeping_item = (selection_index + 1) / (float)(selection_index + 2);

            if (random_value <= chance_of_keeping_item)
            {
                random_value /= chance_of_keeping_item;
            }
            else
            {
                selected_item = candidate_item;
                random_value = (random_value - chance_of_keeping_item) / (1f - chance_of_keeping_item);
            }

            selection_index++;
        }

        /// <summary>
        /// Selects an item in the collection using a predicate with Unity's random, each item that matches the predicate has the same chance of being selected.<br/>
        /// </summary>
        public static bool TryGetRandomItem<TItem>(this IReadOnlyList<TItem> read_only_list, Predicate<TItem> predicate, out TItem item)
        {
            float random_value = UnityEngine.Random.value;
            return TryGetRandomItem(read_only_list, predicate, random_value, out item);
        }

        /// <summary>
        /// Selects an item in the collection using a predicate with a random value, each item that matches the predicate has the same chance of being selected.
        /// </summary>
        /// <param name="random_value">A value inside [0,1]<br />Giving the same value will produce the same result.</param>
        public static bool TryGetRandomItem<TItem>(this IReadOnlyList<TItem> read_only_list, Predicate<TItem> predicate, float random_value, out TItem item)
        {
            int selection_index = 0;
            bool has_first_item = false;
            item = default;

            for (int index = 0; index < read_only_list.Count; index++)
            {
                TItem candidate_item = read_only_list[index];

                if (predicate.Invoke(candidate_item))
                {
                    SelectRandomItem(ref random_value, ref selection_index, ref has_first_item, ref item, candidate_item);
                }
            }

            return has_first_item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static TItem GetRandomItem<TItem>(this IReadOnlyList<TItem> read_only_list, Predicate<TItem> predicate, float random_value)
        {
            TryGetRandomItem(read_only_list, predicate, random_value, out TItem item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static TItem GetRandomItem<TItem>(this IReadOnlyList<TItem> read_only_list, Predicate<TItem> predicate)
        {
            TryGetRandomItem(read_only_list, predicate, out TItem item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static bool TryGetRandomItem<TItem>(this IEnumerable<TItem> enumerable, Predicate<TItem> predicate, float random_value, out TItem item)
        {
            int selection_index = 0;
            bool has_first_item = false;
            item = default;

            foreach (TItem candidate_item in enumerable)
            {
                if (predicate.Invoke(candidate_item))
                {
                    SelectRandomItem(ref random_value, ref selection_index, ref has_first_item, ref item, candidate_item);
                }
            }

            return has_first_item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static bool TryGetRandomItem<TItem>(this IEnumerable<TItem> enumerable, Predicate<TItem> predicate, out TItem item)
        {
            float random_value = UnityEngine.Random.value;
            return TryGetRandomItem(enumerable, predicate, random_value, out item);
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static TItem GetRandomItem<TItem>(this IEnumerable<TItem> enumerable, Predicate<TItem> predicate, float random_value)
        {
            TryGetRandomItem(enumerable, predicate, random_value, out TItem item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static TItem GetRandomItem<TItem>(this IEnumerable<TItem> enumerable, Predicate<TItem> predicate)
        {
            TryGetRandomItem(enumerable, predicate, out TItem item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static bool TryGetRandomItem<TItem>(this HashSet<TItem> hash_set, Predicate<TItem> predicate, float random_value, out TItem item)
        {
            int selection_index = 0;
            bool has_first_item = false;
            item = default;

            foreach (TItem candidate_item in hash_set)
            {
                if (predicate.Invoke(candidate_item))
                {
                    SelectRandomItem(ref random_value, ref selection_index, ref has_first_item, ref item, candidate_item);
                }
            }

            return has_first_item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static bool TryGetRandomItem<TItem>(this HashSet<TItem> hash_set, Predicate<TItem> predicate, out TItem item)
        {
            float random_value = UnityEngine.Random.value;
            return TryGetRandomItem(hash_set, predicate, random_value, out item);
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static TItem GetRandomItem<TItem>(this HashSet<TItem> hash_set, Predicate<TItem> predicate, float random_value)
        {
            TryGetRandomItem(hash_set, predicate, random_value, out TItem item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static TItem GetRandomItem<TItem>(this HashSet<TItem> hash_set, Predicate<TItem> predicate)
        {
            TryGetRandomItem(hash_set, predicate, out TItem item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static bool TryGetRandomItem<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate, float random_value, out KeyValuePair<TKey, TValue> item)
        {
            int selection_index = 0;
            bool has_first_item = false;
            item = default;

            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
            {
                if (predicate.Invoke(kvp))
                {
                    SelectRandomItem(ref random_value, ref selection_index, ref has_first_item, ref item, kvp);
                }
            }

            return has_first_item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static bool TryGetRandomItem<TKey, TValue>(this Dictionary<TKey, TValue> hash_set, Predicate<KeyValuePair<TKey, TValue>> predicate, out KeyValuePair<TKey, TValue> item)
        {
            float random_value = UnityEngine.Random.value;
            return TryGetRandomItem(hash_set, predicate, random_value, out item);
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static KeyValuePair<TKey, TValue> GetRandomItem<TKey, TValue>(this Dictionary<TKey, TValue> hash_set, Predicate<KeyValuePair<TKey, TValue>> predicate, float random_value)
        {
            TryGetRandomItem(hash_set, predicate, random_value, out KeyValuePair<TKey, TValue> item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static KeyValuePair<TKey, TValue> GetRandomItem<TKey, TValue>(this Dictionary<TKey, TValue> hash_set, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            TryGetRandomItem(hash_set, predicate, out KeyValuePair<TKey, TValue> item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static bool TryGetRandomItem<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate, float random_value, out KeyValuePair<TKey, TValue> item)
        {
            int selection_index = 0;
            bool has_first_item = false;
            item = default;

            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
            {
                if (predicate.Invoke(kvp))
                {
                    SelectRandomItem(ref random_value, ref selection_index, ref has_first_item, ref item, kvp);
                }
            }

            return has_first_item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static bool TryGetRandomItem<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> hash_set, Predicate<KeyValuePair<TKey, TValue>> predicate, out KeyValuePair<TKey, TValue> item)
        {
            float random_value = UnityEngine.Random.value;
            return TryGetRandomItem(hash_set, predicate, random_value, out item);
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static KeyValuePair<TKey, TValue> GetRandomItem<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> hash_set, Predicate<KeyValuePair<TKey, TValue>> predicate, float random_value)
        {
            TryGetRandomItem(hash_set, predicate, random_value, out KeyValuePair<TKey, TValue> item);
            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static KeyValuePair<TKey, TValue> GetRandomItem<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> hash_set, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            TryGetRandomItem(hash_set, predicate, out KeyValuePair<TKey, TValue> item);
            return item;
        }
    }
}
