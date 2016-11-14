using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandler<>))]
    internal abstract class CommandHandlerContract<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public abstract bool Reusable { get; }

        public abstract void Handle(ICommand command);

        public abstract Task HandleAsync(ICommand command);
        
        public void Handle(TCommand command)
        {
            Contract.Requires(command != null);

            throw new NotImplementedException();
        }

        public Task HandleAsync(TCommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
