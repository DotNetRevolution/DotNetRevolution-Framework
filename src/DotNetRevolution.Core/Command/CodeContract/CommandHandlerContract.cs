using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command.CodeContract
{
    [ContractClassFor(typeof(ICommandHandler))]
    internal abstract class CommandHandlerContract : ICommandHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(object command)
        {
            Contract.Requires(command != null);
        }
    }
    
    [ContractClassFor(typeof(ICommandHandler<>))]
    internal abstract class CommandHandlerContract<TCommand> : ICommandHandler<TCommand>
        where TCommand : class
    {
        public abstract bool Reusable { get; }

        public void Handle(object command)
        {
        }

        public void Handle(TCommand command)
        {
            Contract.Requires(command != null);
        }
    }
}
