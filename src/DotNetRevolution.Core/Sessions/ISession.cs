using DotNetRevolution.Core.Sessions.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions
{
    [ContractClass(typeof(SessionContract))]
    public interface ISession
    {
        string Identity { get; }

        Dictionary<string, object> Variables { get; }
    }
}
