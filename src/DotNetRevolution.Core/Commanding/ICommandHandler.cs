using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding.CodeContract;

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
    }
}
