﻿using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Extension;
using DotNetRevolution.Core.Serialization;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private readonly IUsernameProvider _usernameProvider;

        public EventStore(IUsernameProvider usernameProvider,
                          ISerializer serializer)
        {     
            Contract.Requires(serializer != null);
            Contract.Requires(usernameProvider != null);
                     
            _usernameProvider = usernameProvider;
        }

        public IEventStream GetEventStream<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot
        {            
            try
            {
                // create new event provider type
                var eventProviderType = new EventProviderType(typeof(TAggregateRoot));
                                
                // get event stream
                var eventStream = GetEventStream(eventProviderType, identity);
                Contract.Assume(eventStream != null);

                // return event stream
                return eventStream;
            }
            catch (Exception ex)
            {
                throw new EventStoreException("Error processing request to get event stream.", ex);
            }
        }

        public void Commit(EventProviderTransaction transaction)
        {
            try
            {                
                var committed = CommitTransaction(_usernameProvider.GetUsername(), transaction);

                var uncommittedRevisions = transaction.EventStream.GetUncommittedRevisions();
                Contract.Assume(uncommittedRevisions != null);

                uncommittedRevisions.ForEach(x => x.Commit(committed));
            }
            catch (Exception ex)
            {
                throw new EventStoreException("Error processing request to commit transaction.", ex);
            }
        }

        protected abstract EventStream GetEventStream(EventProviderType eventProviderType, Identity identity);

        protected abstract DateTime CommitTransaction(string username, EventProviderTransaction transaction);
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {            
            Contract.Invariant(_usernameProvider != null);
        }
    }
}
