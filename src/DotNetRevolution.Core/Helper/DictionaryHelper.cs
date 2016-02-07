using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using DotNetRevolution.Core.Extension;
using System.Globalization;

namespace DotNetRevolution.Core.Helper
{
    public static class DictionaryHelper
    {
        public static string ToString<TKey, TValue>(IDictionary<TKey, TValue> dictionary, string keyValueSeparator, string sequenceSeparator)
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(sequenceSeparator != null);
            Contract.Requires(-sequenceSeparator.Length >= 0);
            Contract.Requires(keyValueSeparator != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var stringBuilder = new StringBuilder();

            dictionary.ForEach(x => stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", x.Key.ToString(), keyValueSeparator, x.Value.ToString(), sequenceSeparator));

            var stringLength = stringBuilder.Length - sequenceSeparator.Length;

            Contract.Assert(stringLength >= 0);

            return stringBuilder.ToString(0, stringLength);
        }
    }
}
