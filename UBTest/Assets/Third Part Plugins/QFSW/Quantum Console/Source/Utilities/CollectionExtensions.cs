using System;
using System.Collections.Generic;

namespace QFSW.QC.Utilities
{
    public static class CollectionExtensions
    {
        /// <summary>Inverts the key/value relationship between the items in the dictionary.</summary>
        /// <returns>Dictionary with the inverted relationship.</returns>
        public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            Dictionary<TValue, TKey> dictionary = new Dictionary<TValue, TKey>();
            foreach (KeyValuePair<TKey, TValue> item in source)
            {
                if (!dictionary.ContainsKey(item.Value))
                {
                    dictionary.Add(item.Value, item.Key);
                }
            }

            return dictionary;
        }

        /// <summary>Gets a sub array of an existing array.</summary>
        /// <param name="index">Index to take the sub array from.</param>
        /// <param name="length">The length of the sub array.</param>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>Skips the last element in the sequence.</summary>
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source)
        {
            using (IEnumerator<T> enumurator = source.GetEnumerator())
            {
                if (enumurator.MoveNext())
                {
                    for (T value = enumurator.Current; enumurator.MoveNext(); value = enumurator.Current)
                    {
                        yield return value;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a distinct stream based on a custom predicate.
        /// </summary>
        /// <typeparam name="TValue">The type of the IEnumerable.</typeparam>
        /// <typeparam name="TDistinct">The type of the value to test for distinctness.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="predicate">The custom distinct item producer.</param>
        /// <returns>The distinct stream.</returns>
        public static IEnumerable<TValue> DistinctBy<TValue, TDistinct>(this IEnumerable<TValue> source, Func<TValue, TDistinct> predicate)
        {
            HashSet<TDistinct> set = new HashSet<TDistinct>();
            foreach (TValue value in source)
            {
                if (set.Add(predicate(value)))
                {
                    yield return value;
                }
            }
        }
    }
}