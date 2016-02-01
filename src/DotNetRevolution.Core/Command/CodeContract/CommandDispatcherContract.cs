using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command.CodeContract
{
    [ContractClassFor(typeof(ICommandDispatcher))]
    internal abstract class CommandDispatcherContract : ICommandDispatcher
    {
        public void UseCatalog(ICommandCatalog catalog)
        {
            Contract.Requires(catalog != null);
        }

        public void Dispatch(object command)
        {
            Contract.Requires(command != null);
        }
    }
}
