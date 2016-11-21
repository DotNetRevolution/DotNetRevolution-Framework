using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandler))]
    internal abstract class CommandHandlerContract : ICommandHandler
    {
        public abstract bool Reusable { get; }

        public ICommandHandlingResult Handle(ICommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandlingResult>() != null);

            throw new NotImplementedException();
        }

        public Task<ICommandHandlingResult> HandleAsync(ICommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task<ICommandHandlingResult>>() != null);

            throw new NotImplementedException();
        }
    }
}
