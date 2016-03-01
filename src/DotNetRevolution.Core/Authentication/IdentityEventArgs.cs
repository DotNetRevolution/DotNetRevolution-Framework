using System;
using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace DotNetRevolution.Core.Authentication
{
    public class IdentityEventArgs : EventArgs
    {
        public IIdentity Identity { get; set; }

        public IdentityEventArgs(IIdentity identity)
        {
            Contract.Requires(identity != null);

            Identity = identity;
        }
    }
}
