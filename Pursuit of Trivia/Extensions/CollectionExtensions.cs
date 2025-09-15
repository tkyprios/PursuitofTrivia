using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.Extensions
{
    /// <summary>
    /// Provides extension methods for collection operations.
    /// </summary>
    internal static class CollectionExtensions
    {
        // Use the same Random instance for all shuffling to avoid timing issues
        private static readonly Random _random = new Random();

        /// <summary>
        /// Shuffles the elements of a collection using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection to shuffle.</param>
        /// <returns>A new List containing the elements in random order.</returns>
        public static List<T> Shuffle<T>(this IEnumerable<T> source)
        {
            // Convert to array for Fisher-Yates shuffle
            var array = source.ToArray();
            int n = array.Length;

            // Fisher-Yates shuffle algorithm
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T temp = array[k];
                array[k] = array[n];
                array[n] = temp;
            }

            return array.ToList();
        }
    }
}
