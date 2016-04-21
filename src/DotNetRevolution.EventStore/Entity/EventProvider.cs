using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventStore.Entity
{
	public class EventProvider
	{
        private readonly ICollection<Transaction> _transactions = new EntityCollection<Transaction>();
        private readonly ICollection<EventProviderDescriptor> _descriptors = new EntityCollection<EventProviderDescriptor>();

        public int EventProviderId { get; }
        public Guid EventProviderGuid { get; }
        public int EventProviderTypeId { get; }
        
        public virtual EventProviderType EventProviderType { get; }

        public virtual EntityCollection<Transaction> Transactions
        {
            get { return _transactions as EntityCollection<Transaction>; }
        }

        public virtual EntityCollection<EventProviderDescriptor> Descriptors
        {
            get { return _descriptors as EntityCollection<EventProviderDescriptor>; }
        }
        
        public EventProvider(IEventStoreAggregateRoot aggregateRoot)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(aggregateRoot.AggregateRootType != null);

            EventProviderGuid = aggregateRoot.Identity.Id;
            EventProviderType = new EventProviderType(aggregateRoot.AggregateRootType.FullName);
        }
        
        public Transaction CreateNewTransaction(IEventStoreAggregateRoot aggregateRoot, string user, object command, Func<object, string> serializeData)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(aggregateRoot.AggregateDescription));
            Contract.Requires(command != null);
            Contract.Requires(serializeData != null);
            Contract.Ensures(Contract.Result<Transaction>() != null);

            var uncommittedEvents = aggregateRoot.UncommittedEvents;
            Contract.Assume(uncommittedEvents != null);

            // create a new transaction
            var transaction = new Transaction(EventProviderId, user, command, uncommittedEvents, serializeData);
            
            // get current event provider description
            var currentDescription = _descriptors.OrderByDescending(x => x.Transaction.Processed).FirstOrDefault();

            // add new event provider description if description is different than latest description
            if (currentDescription == null || currentDescription.Descriptor != aggregateRoot.AggregateDescription)
            {
                _descriptors.Add(new EventProviderDescriptor(this, transaction, aggregateRoot.AggregateDescription));
            }

            // add transaction to collection
            _transactions.Add(transaction);

            // return new transaction
            return transaction;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_descriptors != null);
            Contract.Invariant(_transactions != null);
        }
    }
}