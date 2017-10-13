using System;
using System.Collections.Generic;
using System.Linq;

namespace Conventional.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.Any(predicate) == false;
        }
    }
}