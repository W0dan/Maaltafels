using System;
using System.Collections.Generic;
using System.Linq;

namespace Lalena.UI
{
    public static class ListExtensions
    {
        private static readonly Random Rnd = new Random();

        public static T KiesWillekeurig<T>(this IList<T> range)
        {
            var index = Rnd.Next(0, range.Count());

            return range[index];
        }

        public static T NeemWillekeurig<T>(this IList<T> range)
        {
            var index = Rnd.Next(0, range.Count());
            var result = range[index];

            range.RemoveAt(index);

            return result;
        }

        public static T? TryNeemWillekeurig<T>(this IList<T> range)
            where T : struct 
            => range.Count == 0 
                ? default(T?) 
                : NeemWillekeurig(range);
    }
}