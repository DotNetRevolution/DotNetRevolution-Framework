using DotNetRevolution.Core.Authentication.CodeContract;
using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace DotNetRevolution.Core.Authentication
{
    [ContractClass(typeof(IdentityManagerContract))]
    public interface IIdentityManager
    {
        IIdentity Current { [Pure] get; }
    }
}
