using DotNetRevolution.Core.Sessions.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions
{
    [ContractClass(typeof(CurrentSessionContract))]
    public interface ICurrentSession : ISession
    {
        void SetVariable(string key, object variable);

        void RemoveVariable(string key);
    }
}
