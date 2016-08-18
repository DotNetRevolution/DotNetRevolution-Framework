using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamRevision
    {
        private readonly EventProviderVersion _version;
        private readonly IReadOnlyCollection<IDomainEvent> _domainEvents;
        private DateTime? _committed;
        
        [Pure]
        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEvent>>() != null);

                return _domainEvents;
            }
        }

        [Pure]
        public EventProviderVersion Version
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

                return _version;
            }
        }

        [Pure]
        public Snapshot Snapshot { get; }

        [Pure]
        public DateTime? Committed
        {
            get { return _committed; }            
        }

        public EventStreamRevision(IDomainEventCollection domainEvents)
            : this(EventProviderVersion.Initial, domainEvents)
        {
            Contract.Requires(domainEvents != null);
        }

        public EventStreamRevision(EventProviderVersion version, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(version != null);
            Contract.Requires(domainEvents != null);

            _domainEvents = domainEvents;
            _version = version;
        }

        public EventStreamRevision(EventProviderVersion version, IReadOnlyCollection<IDomainEvent> domainEvents, DateTime committed)
        {
            Contract.Requires(version != null);
            Contract.Requires(domainEvents != null);

            _domainEvents = domainEvents;
            _version = version;
            _committed = committed;
        }

        public EventStreamRevision(EventProviderVersion version, Snapshot snapshot)
        {
            Contract.Requires(version != null);
            Contract.Requires(snapshot != null);
                        
            Snapshot = snapshot;

            _domainEvents = new Collection<IDomainEvent>();
            _version = version;
        }

        public EventStreamRevision(EventProviderVersion version, Snapshot snapshot, DateTime committed)
        {
            Contract.Requires(version != null);
            Contract.Requires(snapshot != null);
            
            Snapshot = snapshot;

            _domainEvents = new Collection<IDomainEvent>();
            _version = version;
            _committed = committed;
        }

        internal void Commit(DateTime dateTime)
        {
            _committed = dateTime;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_version != null);
            Contract.Invariant(_domainEvents != null);
        }
    }
}
