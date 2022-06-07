using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector)
        {
            return list.Distinct(new GenericCompare<T>((a, b) => keySelector(a).Equals(keySelector(b))));
        }
    }
}
