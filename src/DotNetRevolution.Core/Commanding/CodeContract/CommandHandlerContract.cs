using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandler))]
    internal abstract class CommandHandlerContract : ICommandHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(ICommand command)
        {
            Contract.Requires(command != null);

            throw new NotImplementedException();
        }

        public Task HandleAsync(ICommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
