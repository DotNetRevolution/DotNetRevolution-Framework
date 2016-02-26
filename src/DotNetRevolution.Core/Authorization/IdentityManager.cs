using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace DotNetRevolution.Core.Authorization
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

        protected IdentityManager()
        {
        }

        public IdentityManager(IIdentity currentIdentity)
            : this()
        {
            Contract.Requires(currentIdentity != null);

            _currentIdentity = currentIdentity;
        }
    }
}
