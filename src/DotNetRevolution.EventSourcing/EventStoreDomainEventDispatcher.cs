using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public class EventStoreTransactionAnnouncer
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public EventStoreTransactionAnnouncer(IEventStore eventStore, IDomainEventDispatcher dispatcher) 
        {
            Contract.Requires(dispatcher != null);
            Contract.Requires(eventStore != null);

            _dispatcher = dispatcher;

            eventStore.TransactionCommitted += EventStoreTransactionCommitted;
        }

        private void EventStoreTransactionCommitted(object sender, TransactionCommittedEventArgs e)
        {
            _dispatcher.Publish(e.DomainEvents.ToArray());
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}
