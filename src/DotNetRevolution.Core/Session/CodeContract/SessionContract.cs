using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Session.CodeContract
{
    [ContractClassFor(typeof(ISession))]
    public abstract class SessionContract : ISession
    {
        public string Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

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
