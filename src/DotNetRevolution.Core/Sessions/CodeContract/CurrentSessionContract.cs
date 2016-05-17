using DotNetRevolution.Core.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions.CodeContract
{
    [ContractClassFor(typeof(ICurrentSession))]
    internal abstract class CurrentSessionContract : ICurrentSession
    {
        public string Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IReadOnlyDictionary<string, object> Variables
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void SetVariable(string key, object variable)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(key) == false);
            Contract.Ensures(Variables.PureContainsKey(key));
        }

        public void RemoveVariable(string key)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(key) == false);
            Contract.Ensures(Variables.PureContainsKey(key) == false);
        }
    }
}
