using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionEventStoreTransactionAnnouncer : EventStoreTransactionAnnouncer
    {
        private readonly IProjectionDispatcher _dispatcher;

        public ProjectionEventStoreTransactionAnnouncer(IEventStore eventStore, IProjectionDispatcher dispatcher) 
            : base(eventStore)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(dispatcher != null);

            _dispatcher = dispatcher;
        }

        protected override void TransactionCommitted(EventProviderTransaction transaction)
        {            
            _dispatcher.Dispatch(transaction);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}