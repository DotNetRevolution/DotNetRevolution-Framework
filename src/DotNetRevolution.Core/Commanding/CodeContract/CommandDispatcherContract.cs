using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandDispatcher))]
    internal abstract class CommandDispatcherContract : ICommandDispatcher
    {
        public void UseCatalog(ICommandCatalog catalog)
        {
            Contract.Requires(catalog != null);
        }

        public void Dispatch(ICommand command)
        {
            Contract.Requires(command != null);
        }

        public Task DispatchAsync(ICommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
