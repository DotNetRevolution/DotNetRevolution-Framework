using System;
using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace DotNetRevolution.Core.Authentication.CodeContract
{
    [ContractClassFor(typeof(IIdentityManager))]
    public abstract class IdentityManagerContract : IIdentityManager
    {
        public IIdentity Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
