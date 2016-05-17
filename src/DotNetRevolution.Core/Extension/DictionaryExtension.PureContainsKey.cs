using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Extension
{
    public static partial class DictionaryExtension
    {
        [Pure]
        public static bool PureContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(ReferenceEquals(key, null) == false);

            return dictionary.ContainsKey(key);
        }

        [Pure]
        public static bool PureContainsKey<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(ReferenceEquals(key, null) == false);

            return dictionary.ContainsKey(key);
        }
    }
}
