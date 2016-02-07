using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Helper;
using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Logging
{
    [Serializable]
    public class LogEntryContextDictionary : Dictionary<string, string>
    {
        private const string KeyValueSeparator = "=";
        private const string SequenceSeparator = "|";

        protected LogEntryContextDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {            
        }

        public override string ToString()
        {
            Contract.Assume(-1 >= 0);
            
            return DictionaryHelper.ToString(this, KeyValueSeparator, SequenceSeparator);
        }
    }
}
