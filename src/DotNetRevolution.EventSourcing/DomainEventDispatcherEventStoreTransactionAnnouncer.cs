using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public class DomainEventDispatcherEventStoreTransactionAnnouncer : EventStoreTransactionAnnouncer
    {
        private readonly IDomainEventDispatcher _dispatcher;
        
        public DomainEventDispatcherEventStoreTransactionAnnouncer(IEventStore eventStore, IDomainEventDispatcher dispatcher) 
            : base(eventStore)
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(eventStore != null);

            _dispatcher = dispatcher;
        }

        protected override void TransactionCommitted(EventProviderTransaction transaction)
        {
            _dispatcher.Publish(transaction.GetDomainEvents().ToArray());
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}
