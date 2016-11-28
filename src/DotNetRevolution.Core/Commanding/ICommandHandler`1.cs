using DotNetRevolution.Core.Commanding.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerContract<>))]
    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        ICommandHandlingResult Handle(ICommandHandlerContext<TCommand> context);

        /// <summary>
        /// Handles the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<TCommand> context);
    }
}
