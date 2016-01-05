using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Command.CodeContract;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandHandlerContract))]
    public interface ICommandHandler
    {
        bool Reusable { [Pure] get; }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Handle(object command);
    }

    [ContractClass(typeof(CommandHandlerContract<>))]
    public interface ICommandHandler<in TCommand>  : ICommandHandler
        where TCommand : class
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Handle(TCommand command);
    }
}
