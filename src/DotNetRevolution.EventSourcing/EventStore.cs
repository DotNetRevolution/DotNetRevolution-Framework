using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Serialization;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private readonly IEventStreamProcessorProvider _eventStreamProcessorProvider;
        private readonly ISerializer _serializer;

        public EventStore(IEventStreamProcessorProvider eventStreamProcessorProvider)
        {
            Contract.Requires(eventStreamProcessorProvider != null);
                        
            _eventStreamProcessorProvider = eventStreamProcessorProvider;
        }

        public EventProvider GetEventProvider<TAggregateRoot>(Identity identity) where TAggregateRoot : class
        {            
            try
            {
                // create new event provider type
                var eventProviderType = new EventProviderType(typeof(TAggregateRoot));

                EventProviderVersion version;
                EventProviderDescriptor descriptor;

                // get domain events
                var eventStream = GetDomainEvents(eventProviderType, identity, out version, out descriptor);

                // get event stream processor
                var eventStreamProcessor = _eventStreamProcessorProvider.GetProcessor(eventProviderType);

                // return new event provider with gathered information
                return new EventProvider(eventProviderType, 
                                         identity, 
                                         version,
                                         descriptor,
                                         eventStream,
                                         eventStreamProcessor);
            }
            catch (Exception ex)
            {
                throw new EventStoreException("Error processing request to get event stream.", ex);
            }
        }

        public void Commit(Transaction transaction)
        {
            Contract.Requires(transaction != null);

            try
            {
                CommitTransaction(transaction);
            }
            catch (Exception ex)
            {
                throw new EventStoreException("Error processing request to commit transaction.", ex);
            }
        }

        protected abstract EventStream GetDomainEvents(EventProviderType eventProviderType, Identity identity, out EventProviderVersion version, out EventProviderDescriptor eventProviderDescriptor);

        protected abstract void CommitTransaction(Transaction transaction);
    }
}
