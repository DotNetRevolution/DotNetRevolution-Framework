using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventStore.Entity
{
    public class Transaction
    {
        private readonly ICollection<TransactionEvent> _events;
        private readonly ICollection<TransactionAnnouncement> _announcements;

        public long TransactionId { get; }
        public Guid EventProviderId { get; }
        public string User { get; }
        public string Application { get; }
        public DateTime Processed { get; }
        
        public TransactionCommand TransactionCommand { get; }

        public virtual EntityCollection<TransactionAnnouncement> Announcements
        {
            get { return _announcements as EntityCollection<TransactionAnnouncement>; }
        }

        public virtual EntityCollection<TransactionEvent> Events
        {
            get { return _events as EntityCollection<TransactionEvent>; }
        }

        protected Transaction()
        {
            _events = new EntityCollection<TransactionEvent>();
            _announcements = new EntityCollection<TransactionAnnouncement>();
        }

        internal Transaction(Guid eventProviderId, string user, TransactionCommandType transactionCommandType, string commandData, IEnumerable<object> events, Func<object, string> serializeData)
            : this()
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(commandData));
            Contract.Requires(eventProviderId != Guid.Empty);
            Contract.Requires(transactionCommandType != null);
            Contract.Requires(events != null);
            Contract.Requires(serializeData != null);
            
            EventProviderId = eventProviderId;

            TransactionCommand = new TransactionCommand(this, transactionCommandType, commandData);

            if (!string.IsNullOrWhiteSpace(user))
            {
                User = user;
            }
            
            AddEvents(events, serializeData);
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

        private void AddEvents(IEnumerable<object> events, Func<object, string> serializeData)
        {
            Contract.Requires(serializeData != null);
            Contract.Requires(events != null);

            var sequence = -1;

            // loop through each event to add to the collection
            foreach (var @event in events)
            {
                Contract.Assume(@event != null);

                var eventType = @event.GetType();

                // add a new transaction event to the collection
                _events.Add(new TransactionEvent(this, ++sequence, new TransactionEventType(eventType.FullName), serializeData(@event)));                
            }
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_events != null);
            Contract.Invariant(_announcements != null);
        }
    }
}