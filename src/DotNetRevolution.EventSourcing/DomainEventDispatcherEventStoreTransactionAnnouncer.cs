﻿using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Metadata;
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
            // get domain events
            var domainEvents = transaction.GetDomainEvents();

            // get metadata
            var metadata = new MetaCollection(transaction.Metadata);

            // create contexts
            var contexts = domainEvents.Select(x => new DomainEventHandlerContext(x, metadata))
                                       .ToArray();

            // public contexts
            _dispatcher.Publish(contexts);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}
