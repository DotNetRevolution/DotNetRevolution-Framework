using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Extension
{
    public static partial class DictionaryExtension
    {
        [Pure]
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            Contract.Requires(dictionary != null);
            Contract.Ensures(Contract.Result<IReadOnlyDictionary<TKey, TValue>>() != null);

            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
