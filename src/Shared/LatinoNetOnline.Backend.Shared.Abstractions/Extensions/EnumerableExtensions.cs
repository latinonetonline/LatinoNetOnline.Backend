
using System.Collections.Generic;
using System.Linq;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ConvertToEnumerable<T>(this T obj)
        {
            return new[] { obj };
        }

        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }

            yield return value;
        }

        public static IEnumerable<string> WhereIsNotEmply(this IEnumerable<string> enumerable)
        {
            return enumerable.Where(x => !string.IsNullOrWhiteSpace(x));
        }
    }
}
