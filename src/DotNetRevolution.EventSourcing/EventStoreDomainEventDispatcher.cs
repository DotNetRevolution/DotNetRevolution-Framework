using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStoreDomainEventDispatcher : DomainEventDispatcher
    {
        public EventStoreDomainEventDispatcher(IEventStore eventStore, 
                                               IDomainEventHandlerFactory handlerFactory) 
            : base(handlerFactory)
        {
            Contract.Requires(handlerFactory != null);
            Contract.Requires(eventStore != null);

            eventStore.TransactionCommitted += EventStoreTransactionCommitted;
        }

        private void EventStoreTransactionCommitted(object sender, TransactionCommittedEventArgs e)
        {
            PublishAll(e.DomainEvents);
        }
    }
}
