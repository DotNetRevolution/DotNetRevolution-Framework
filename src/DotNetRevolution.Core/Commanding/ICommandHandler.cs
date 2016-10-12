using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding.CodeContract;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerContract))]
    public interface ICommandHandler
    {
        bool Reusable { [Pure] get; }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Handle(ICommand command);

        /// <summary>
        /// Handles the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        Task HandleAsync(ICommand command);
    }
}
