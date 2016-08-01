using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Serialization;
using DotNetRevolution.EventSourcing.AggregateRoot;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private readonly IAggregateRootProcessorFactory _aggregateRootProcessorFactory;
        private readonly ISnapshotPolicyFactory _snapshotPolicyFactory;
        private readonly ISnapshotProviderFactory _snapshotProviderFactory;
        private readonly IUsernameProvider _usernameProvider;

        public EventStore(IAggregateRootProcessorFactory aggregateRootProcessorFactory,
                          ISnapshotPolicyFactory snapshotPolicyFactory,
                          ISnapshotProviderFactory snapshotProviderFactory,
                          IUsernameProvider usernameProvider,
                          ISerializer serializer)
        {
            Contract.Requires(aggregateRootProcessorFactory != null);
            Contract.Requires(snapshotPolicyFactory != null);
            Contract.Requires(snapshotProviderFactory != null);
            Contract.Requires(serializer != null);
            Contract.Requires(usernameProvider != null);

            _aggregateRootProcessorFactory = aggregateRootProcessorFactory;
            _snapshotPolicyFactory = snapshotPolicyFactory;
            _snapshotProviderFactory = snapshotProviderFactory;
            _usernameProvider = usernameProvider;
        }

        public EventProvider<TAggregateRoot> GetEventProvider<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot
        {            
            try
            {
                // create new event provider type
                var eventProviderType = new EventProviderType(typeof(TAggregateRoot));

                EventProviderVersion version;
                EventProviderDescriptor descriptor;
                Snapshot snapshot;

                // get domain events
                var eventStream = GetDomainEvents(eventProviderType, identity, out version, out descriptor, out snapshot);
                Contract.Assume(eventStream != null);
                Contract.Assume(version != null);
                Contract.Assume(descriptor != null);

                // get processor
                var aggregateRootProcessor = _aggregateRootProcessorFactory.GetProcessor(eventProviderType);

                // get snapshot provider
                var snapshotProvider = _snapshotProviderFactory.GetProvider(eventProviderType);

                // return new event provider with gathered information
                return new EventProvider<TAggregateRoot>(eventProviderType, 
                                         identity, 
                                         version,
                                         descriptor,
                                         eventStream,
                                         aggregateRootProcessor,
                                         snapshotProvider);
            }
            catch (Exception ex)
            {
                throw new EventStoreException("Error processing request to get event stream.", ex);
            }
        }

        public void Commit(Transaction transaction)
        {
            try
            {                
                CommitTransaction(_usernameProvider.GetUsername(), transaction);
            }
            catch (Exception ex)
            {
                throw new EventStoreException("Error processing request to commit transaction.", ex);
            }
        }
        
        public Snapshot GetSnapshotIfPolicySatisfied(IEventProvider eventProvider)
        {
            Contract.Requires(eventProvider != null);

            // get snapshot policy
            var snapshotPolicy = _snapshotPolicyFactory.GetPolicy(eventProvider.EventProviderType);

            // check event provider against policy
            if (snapshotPolicy.Check(eventProvider))
            {
                // ask event provider for a snapshot
                return eventProvider.GetSnapshot();
            }

            // policy determined no snapshot needed
            return null;
        }
        
        protected abstract EventStream GetDomainEvents(EventProviderType eventProviderType, Identity identity, out EventProviderVersion version, out EventProviderDescriptor eventProviderDescriptor, out Snapshot snapshot);

        protected abstract void CommitTransaction(string username, Transaction transaction);
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_aggregateRootProcessorFactory != null);
            Contract.Invariant(_snapshotPolicyFactory != null);
            Contract.Invariant(_snapshotProviderFactory != null);
            Contract.Invariant(_usernameProvider != null);
        }
    }
}
