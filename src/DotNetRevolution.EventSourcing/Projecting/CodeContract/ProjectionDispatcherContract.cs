using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionDispatcher))]
    internal abstract class ProjectionDispatcherContract : IProjectionDispatcher
    {
        public void Dispatch(EventProviderTransaction eventProviderTransaction)
        {
            Contract.Requires(eventProviderTransaction != null);

            throw new NotImplementedException();
        }

        public void Dispatch(params EventProviderTransaction[] eventProviderTransactions)
        {
            Contract.Requires(eventProviderTransactions != null);
            
            throw new NotImplementedException();
        }
    }
}
