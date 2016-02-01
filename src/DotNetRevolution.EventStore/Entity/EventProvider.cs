using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventStore.Entity
{
	public class EventProvider
	{
        private readonly ICollection<Transaction> _transactions;
        private readonly ICollection<EventProviderDescriptor> _descriptions;
        
        public Guid EventProviderId { get; private set; }
        public int EventProviderTypeId { get; private set; }
        
        public virtual EventProviderType EventProviderType { get; private set; }

        public virtual EntityCollection<Transaction> Transactions
        {
            get { return _transactions as EntityCollection<Transaction>; }
        }

        public virtual EntityCollection<EventProviderDescriptor> Descriptions
        {
            get { return _descriptions as EntityCollection<EventProviderDescriptor>; }
        }

	    private EventProvider()
        {
            _transactions = new EntityCollection<Transaction>();
            _descriptions = new EntityCollection<EventProviderDescriptor>();
        }

        public EventProvider(Guid eventProviderId, EventProviderType type)
            : this()
        {
            Contract.Requires(eventProviderId != Guid.Empty);
            Contract.Requires(type != null);

            EventProviderId = eventProviderId;
            EventProviderType = type;
        }
        
        public Transaction CreateNewTransaction(string eventProviderDescription, string user, object command, TransactionCommandType transactionCommandType, IEnumerable<object> events, Func<object, string> serializeData)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(eventProviderDescription));
            Contract.Requires(command != null);
            Contract.Requires(transactionCommandType != null);
            Contract.Requires(serializeData != null);
            Contract.Requires(events != null);
            Contract.Ensures(Contract.Result<Transaction>() != null);

            Contract.Assume(EventProviderId != Guid.Empty);

            var commandData = serializeData(command);
            Contract.Assume(!string.IsNullOrWhiteSpace(commandData));
            
            // create a new transaction
            var transaction = new Transaction(EventProviderId, user, transactionCommandType, commandData, events, serializeData);
            
            // get current event provider description
            var currentDescription = _descriptions.OrderByDescending(x => x.Transaction.Processed).FirstOrDefault();

            // add new event provider description if description is different than latest description
            if (currentDescription == null || currentDescription.Descriptor != eventProviderDescription)
            {
                _descriptions.Add(new EventProviderDescriptor(this, transaction, eventProviderDescription));
            }

            // add transaction to collection
            _transactions.Add(transaction);

            // return new transaction
            return transaction;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_descriptions != null);
            Contract.Invariant(_transactions != null);
        }
    }
}