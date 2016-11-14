using DotNetRevolution.Core.Commanding.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerContract<>))]
    public interface ICommandHandler<in TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Handle(TCommand command);

        /// <summary>
        /// Handles the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        Task HandleAsync(TCommand command);
    }
}
