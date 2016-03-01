using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace DotNetRevolution.Core.Authentication
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IIdentity _currentIdentity;

        public virtual IIdentity Current
        {
            get
            {
                return _currentIdentity;
            }
        }
        
        public IdentityManager(IIdentity currentIdentity)
        {
            Contract.Requires(currentIdentity != null);

            _currentIdentity = currentIdentity;
        }
    }
}
