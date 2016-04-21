using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventStore.Entity
{
    public class Transaction
    {
        private readonly ICollection<TransactionEvent> _events = new EntityCollection<TransactionEvent>();
        private readonly ICollection<TransactionAnnouncement> _announcements = new EntityCollection<TransactionAnnouncement>();
        private readonly ICollection<TransactionEventProvider> _transactionEventProviders = new EntityCollection<TransactionEventProvider>();

        private int _eventSequence = -1;

        public long TransactionId { get; }
        public string User { get; }
        public string Application { get; }
        public DateTime Processed { get; }
        
        public TransactionCommand TransactionCommand { get; }

        public virtual EntityCollection<TransactionAnnouncement> Announcements
        {
            get { return _announcements as EntityCollection<TransactionAnnouncement>; }
        }

        public virtual EntityCollection<TransactionEventProvider> EventProviders
        {
            get { return _transactionEventProviders as EntityCollection<TransactionEventProvider>; }
        }

        public virtual EntityCollection<TransactionEvent> Events
        {
            get { return _events as EntityCollection<TransactionEvent>; }
        }
        
        internal Transaction(int eventProviderId, string user, object command, IEnumerable<object> events, Func<object, string> serializeData)
        {
            Contract.Requires(command != null);
            Contract.Requires(events != null);
            Contract.Requires(serializeData != null);

            var commandData = serializeData(command);
            Contract.Assume(!string.IsNullOrWhiteSpace(commandData));

            TransactionCommand = new TransactionCommand(this, new TransactionCommandType(command.GetType().FullName), commandData);

            if (!string.IsNullOrWhiteSpace(user))
            {
                User = user;
            }
            
            AddEvents(eventProviderId, events, serializeData);
        }
                
        public void Announce(Action<TransactionEvent> announceEvent)
        {
            Contract.Requires(announceEvent != null);
            
            var announcement = new TransactionAnnouncement(this);

            _announcements.Add(announcement);

            // sort events to announce in order
            foreach (var transactionEvent in _events.OrderBy(x => x.Sequence))
            {
                try
                {
                    announceEvent(transactionEvent);
                }
                catch (Exception exception)
                {
                    announcement.RecordError(transactionEvent, exception.GetType().FullName, exception.ToString());
                    throw;
                }
            }
        }

        public void AddEvents(int eventProviderId, IEnumerable<object> events, Func<object, string> serializeData)
        {
            Contract.Requires(serializeData != null);
            Contract.Requires(events != null);

            var transactionEventProvider = AddOrReturnTransactionEventProvider(eventProviderId);

            // loop through each event to add to the collection
            foreach (var @event in events)
            {
                Contract.Assume(@event != null);

                var eventType = @event.GetType();

                var eventData = serializeData(@event);
                Contract.Assume(!string.IsNullOrWhiteSpace(eventData));

                // add a new transaction event to the collection
                _events.Add(new TransactionEvent(transactionEventProvider, ++_eventSequence, new TransactionEventType(eventType.FullName), eventData));                
            }
        }

        private TransactionEventProvider AddOrReturnTransactionEventProvider(int eventProviderId)
        {
            Contract.Ensures(Contract.Result<TransactionEventProvider>() != null);

            var transactionEventProvider = _transactionEventProviders.FirstOrDefault(x => x.EventProviderId == eventProviderId);

            if (transactionEventProvider == null)
            {
                transactionEventProvider = new TransactionEventProvider(this, eventProviderId);
                _transactionEventProviders.Add(transactionEventProvider);
            }
            
            return transactionEventProvider;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_events != null);
            Contract.Invariant(_announcements != null);
            Contract.Invariant(_transactionEventProviders != null);
        }
    }
}