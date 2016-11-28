using DotNetRevolution.Core.Commanding.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerContextFactoryContract))]
    public interface ICommandHandlerContextFactory
    {
        [Pure]
        ICommandHandlerContext GetContext(ICommand command);

        [Pure]
        ICommandHandlerContext<TCommand> GetContext<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
