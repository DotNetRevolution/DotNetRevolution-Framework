using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public class EventStream
    {
        private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();

        public Identity EventProviderId { get; }

        public int MinVersion { get; }

        public int MaxVersion { get; }

        public IReadOnlyCollection<IDomainEvent> Events
        {
            get
            {
                return CommittedEvents
                    .Union(_uncommittedEvents)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public IReadOnlyCollection<IDomainEvent> CommittedEvents { get; }

        public IReadOnlyCollection<IDomainEvent> UncommittedEvents
        {
            get
            {
                return _uncommittedEvents.AsReadOnly();
            }
        }

        public EventStream(Identity eventProviderId, int minVersion, int maxVersion, IReadOnlyCollection<IDomainEvent> committedEvents)
        {
            Contract.Requires(eventProviderId != null);
            Contract.Requires(committedEvents != null);
            Contract.Requires(Contract.ForAll(committedEvents, o => o != null));

            EventProviderId = eventProviderId;
            MinVersion = minVersion;
            MaxVersion = maxVersion;
            CommittedEvents = committedEvents;
        }

        public void AddEvents(IDomainEventCollection domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Requires(Contract.ForAll(domainEvents, o => o != null));

            _uncommittedEvents.AddRange(domainEvents);
        }

        public TEventProvider Run<TEventProvider>() where TEventProvider : class
        {
            return new EventStreamProcessor().Process<TEventProvider>(this);
        }
    }
}
