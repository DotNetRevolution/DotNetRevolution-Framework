using DotNetRevolution.Core.Authorization.CodeContract;
using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace DotNetRevolution.Core.Authorization
{
    [ContractClass(typeof(IdentityManagerContract))]
    public interface IIdentityManager
    {
        IIdentity Current { [Pure] get; }
    }
}
