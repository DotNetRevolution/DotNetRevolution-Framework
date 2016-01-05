using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Command.CodeContract;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandDispatcherContract))]
    public interface ICommandDispatcher
    {
        void Dispatch(object command);
    }
}
