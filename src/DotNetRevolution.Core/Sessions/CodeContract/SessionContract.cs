using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions.CodeContract
{
    [ContractClassFor(typeof(ISession))]
    internal abstract class SessionContract : ISession
    {
        public string Identity
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                throw new NotImplementedException();
            }
        }

        public Dictionary<string, object> Variables
        {
            get
            {
                Contract.Ensures(Contract.Result<Dictionary<string, object>>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
