using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Helper;

namespace DotNetRevolution.Core.Extension
{
    public static partial class DictionaryExtension
    {
        private static readonly DictionaryHelper DictionaryHelper;

        static DictionaryExtension()
        {
            DictionaryHelper = new DictionaryHelper();
        }

        public static string ToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string keyValueSeparator, string sequenceSeparator)
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(keyValueSeparator != null);
            Contract.Requires(sequenceSeparator != null);
            Contract.Requires(-sequenceSeparator.Length >= 0);
            Contract.Ensures(Contract.Result<string>() != null);

            return DictionaryHelper.ToString(dictionary, keyValueSeparator, sequenceSeparator);
        }

        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string keyValueSeparator, string sequenceSeparator)
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(keyValueSeparator != null);
            Contract.Requires(sequenceSeparator != null);
            Contract.Requires(-sequenceSeparator.Length >= 0);
            Contract.Ensures(Contract.Result<string>() != null);

            return DictionaryHelper.ToString(dictionary, keyValueSeparator, sequenceSeparator);
        }
    }
}
