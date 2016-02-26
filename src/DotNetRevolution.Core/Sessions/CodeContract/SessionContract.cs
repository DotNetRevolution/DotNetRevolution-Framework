using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions.CodeContract
{
    [ContractClassFor(typeof(ISession))]
    internal abstract class SessionContract : ISession
    {
        public string Id
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                throw new NotImplementedException();
            }
        }

        public IReadOnlyDictionary<string, object> Variables
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyDictionary<string, object>>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
