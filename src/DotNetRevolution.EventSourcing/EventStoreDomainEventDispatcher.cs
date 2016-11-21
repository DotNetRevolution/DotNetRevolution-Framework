using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public class EventStoreTransactionAnnouncer
    {
        private readonly IDomainEventDispatcher[] _dispatchers;

        public EventStoreTransactionAnnouncer(IEventStore eventStore, params IDomainEventDispatcher[] dispatchers) 
        {
            Contract.Requires(dispatchers != null);
            Contract.Requires(eventStore != null);

            _dispatchers = dispatchers;

            eventStore.TransactionCommitted += EventStoreTransactionCommitted;
        }

        private void EventStoreTransactionCommitted(object sender, TransactionCommittedEventArgs e)
        {
            for(var i = 0; i < _dispatchers.Length; i++)
            {
                var dispatcher = _dispatchers[i];
                Contract.Assume(dispatcher != null);

                dispatcher.Publish(e.DomainEvents.ToArray());
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatchers != null);
        }
    }
}
