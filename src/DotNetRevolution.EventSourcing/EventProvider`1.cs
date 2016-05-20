using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.AggregateRoot;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider<TAggregateRoot> : EventProvider, IEventProvider<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IAggregateRootProcessor _aggregateRootProcessor;
        private readonly ISnapshotProvider _snapshotProvider;

        private Snapshot _snapshot;

        public EventProvider(EventProviderType type,
            Identity identity,
            EventProviderVersion version,
            EventProviderDescriptor descriptor,
            EventStream domainEvents,
            IAggregateRootProcessor aggregateRootProcessor)
            : base(type, identity, version, descriptor, domainEvents)
        {
            Contract.Requires(type != null);
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(descriptor != null);
            Contract.Requires(domainEvents != null);
            Contract.Requires(aggregateRootProcessor != null);            

            _aggregateRootProcessor = aggregateRootProcessor;
        }
        
        public EventProvider(EventProviderType type,
            Identity identity,
            EventProviderVersion version,
            EventProviderDescriptor descriptor,
            EventStream domainEvents,
            IAggregateRootProcessor aggregateRootProcessor,
            ISnapshotProvider snapshotProvider)
            : base(type, identity, version, descriptor, domainEvents)
        {
            Contract.Requires(type != null);
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(descriptor != null);
            Contract.Requires(domainEvents != null);
            Contract.Requires(aggregateRootProcessor != null);

            _aggregateRootProcessor = aggregateRootProcessor;
            _snapshotProvider = snapshotProvider;
        }

        public EventProvider(IDomainEventCollection domainEventCollection, IAggregateRootProcessor aggregateRootProcessor, ISnapshotProvider snapshotProvider)
            : this(new EventProviderType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity,
                   EventProviderVersion.Initial,
                   new EventProviderDescriptor(domainEventCollection.AggregateRoot.ToString()),
                   new EventStream(domainEventCollection),
                   aggregateRootProcessor,
                   snapshotProvider)
        {
            Contract.Requires(domainEventCollection?.AggregateRoot != null);
            Contract.Requires(string.IsNullOrWhiteSpace(domainEventCollection.AggregateRoot.ToString()) == false);
            Contract.Requires(Contract.ForAll(domainEventCollection, o => o != null));
            Contract.Requires(aggregateRootProcessor != null);            
        }

        public EventProvider(IDomainEventCollection domainEventCollection, IAggregateRootProcessor aggregateRootProcessor)
            : this(new EventProviderType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity,
                   EventProviderVersion.Initial,
                   new EventProviderDescriptor(domainEventCollection.AggregateRoot.ToString()),
                   new EventStream(domainEventCollection),
                   aggregateRootProcessor)
        {
            Contract.Requires(domainEventCollection?.AggregateRoot != null);
            Contract.Requires(string.IsNullOrWhiteSpace(domainEventCollection.AggregateRoot.ToString()) == false);
            Contract.Requires(Contract.ForAll(domainEventCollection, o => o != null));
            Contract.Requires(aggregateRootProcessor != null);
        }

        public TAggregateRoot CreateAggregateRoot()
        {
            Contract.Assume(Contract.ForAll(DomainEvents, o => o != null));

            TAggregateRoot aggregateRoot;

            // run through events if snapshot is null
            if (_snapshot == null)
            {
                // process events
                aggregateRoot = _aggregateRootProcessor.Process<TAggregateRoot>(DomainEvents);

                _snapshot = CreateSnapshot(aggregateRoot);
            }
            else
            {
                // snapshot created because events already ran, use snapshot to create aggregate root
                aggregateRoot = _aggregateRootProcessor.Process<TAggregateRoot>(_snapshot);
            }

            return aggregateRoot;
        }

        private Snapshot CreateSnapshot()
        {
            // make sure this event provider can snapshot
            if (_snapshotProvider == null)
            {
                return null;
            }

            // create aggregate root
            var aggregateRoot = _aggregateRootProcessor.Process<TAggregateRoot>(DomainEvents);

            // create and return snapshot from aggregate root
            return CreateSnapshot(aggregateRoot);
        }

        private Snapshot CreateSnapshot(TAggregateRoot aggregateRoot)
        {
            Contract.Requires(aggregateRoot != null);
            
            // make sure this event provider can snapshot
            if (_snapshotProvider == null)
            {
                return null;
            }

            // create and return snapshot from aggregate root
            return _snapshotProvider.GetSnapshot(aggregateRoot);
        }

        public EventProvider<TAggregateRoot> CreateNewVersion(IDomainEventCollection domainEvents)
        {
            var aggregateRoot = domainEvents.AggregateRoot;
            
            // make sure aggregate root type is same as this event provider type
            if (new EventProviderType(aggregateRoot.GetType()) != EventProviderType)
            {                
                throw new InvalidOperationException("Event provider type and aggregate root type do not match.");
            }

            // make sure aggregate root identity is the same as this event provider identity
            if (aggregateRoot.Identity != Identity)
            {
                throw new InvalidOperationException("Event provider identity and aggregate root identity do not match.");
            }
            
            // return new event provider with incremented version
            return new EventProvider<TAggregateRoot>(EventProviderType,
                Identity,
                Version.Increment(),
                new EventProviderDescriptor(aggregateRoot.ToString()),
                new EventStream(domainEvents, _snapshot),
                _aggregateRootProcessor,
                _snapshotProvider);
        }

        public override Snapshot GetSnapshot()
        {
            // check for existing snapshot
            if (_snapshot == null)
            {
                // create and save snapshot
                return _snapshot = CreateSnapshot();
            }

            // return already created snapshot
            return _snapshot;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_aggregateRootProcessor != null);            
        }
    }
}
