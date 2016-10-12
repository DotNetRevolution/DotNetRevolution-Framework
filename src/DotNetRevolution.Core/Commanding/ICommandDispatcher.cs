using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding.CodeContract;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandDispatcherContract))]
    public interface ICommandDispatcher
    {
        void Dispatch(ICommand command);

        Task DispatchAsync(ICommand command);
    }
}
