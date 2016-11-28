using DotNetRevolution.Core.Commanding.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerContextContract<>))]
    public interface ICommandHandlerContext<TCommand> : ICommandHandlerContext
        where TCommand : ICommand
    {
        [Pure]
        new TCommand Command { get; }
    }
}
