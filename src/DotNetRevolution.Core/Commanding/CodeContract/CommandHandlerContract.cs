using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandler))]
    internal abstract class CommandHandlerContract : ICommandHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(ICommand command)
        {
            Contract.Requires(command != null);
        }
    }

    [ContractClassFor(typeof(ICommandHandler<>))]
    internal abstract class CommandHandlerContract<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public abstract bool Reusable { get; }

        public void Handle(ICommand command)
        {
        }

        public void Handle(TCommand command)
        {
            Contract.Requires(command != null);
        }
    }

    [ContractClassFor(typeof(CommandHandler<>))]
    internal abstract class CommandHandlerBaseContract<TCommand> : CommandHandler<TCommand>
        where TCommand : ICommand
    {
        public override void Handle(TCommand command)
        {
            Contract.Requires(command != null);

            throw new NotImplementedException();
        }
    }
}
