using DotNetRevolution.Core.Session.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Session
{
    [ContractClass(typeof(SessionContract))]
    public interface ISession
    {
        string Identity { get; }

        Dictionary<string, object> Variables { get; }
    }
}