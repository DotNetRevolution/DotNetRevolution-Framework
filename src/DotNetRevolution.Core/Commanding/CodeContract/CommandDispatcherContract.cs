using System.Diagnostics.Contracts;

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
    }
}
