using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding.CodeContract;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandDispatcherContract))]
    public interface ICommandDispatcher
    {
        void Dispatch(ICommand command);
    }
}
