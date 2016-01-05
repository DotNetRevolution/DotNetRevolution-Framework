using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Helper;

namespace DotNetRevolution.Core.Logging
{
    public class LogEntryContext : Dictionary<string, string>
    {
        private const string KeyValueSeparator = "=";
        private const string SequenceSeparator = "|";

        public override string ToString()
        {
            Contract.Assume(-1 >= 0);

            var dictionaryHelper = new DictionaryHelper();

            return dictionaryHelper.ToString(this, KeyValueSeparator, SequenceSeparator);
        }
    }
}
