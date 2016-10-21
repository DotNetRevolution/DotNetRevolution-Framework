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

        public abstract ICommandHandlingResult Handle(ICommand command);

        public abstract Task<ICommandHandlingResult> HandleAsync(ICommand command);
        
        public ICommandHandlingResult Handle(TCommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandlingResult>() != null);

            throw new NotImplementedException();
        }

        public Task<ICommandHandlingResult> HandleAsync(TCommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task<ICommandHandlingResult>>() != null);

            throw new NotImplementedException();
        }
    }
}
