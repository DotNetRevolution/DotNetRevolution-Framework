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

        public abstract ICommandHandlingResult Handle(ICommandHandlerContext context);

        public abstract Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext context);
        
        public ICommandHandlingResult Handle(ICommandHandlerContext<TCommand> context)
        {
            Contract.Requires(context != null);
            Contract.Ensures(Contract.Result<ICommandHandlingResult>() != null);

            throw new NotImplementedException();
        }

        public Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<TCommand> context)
        {
            Contract.Requires(context != null);
            Contract.Ensures(Contract.Result<Task<ICommandHandlingResult>>() != null);

            throw new NotImplementedException();
        }
    }
}
