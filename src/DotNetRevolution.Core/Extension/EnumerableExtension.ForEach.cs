using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Extension
{
    public static partial class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            Contract.Requires(items != null);
            Contract.Requires(action != null);

            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}
