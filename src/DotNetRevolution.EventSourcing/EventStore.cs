using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStore
    {
        private readonly ISerializer _serializer;

        public EventStore(ISerializer serializer)
        {
            Contract.Requires(serializer != null);

            _serializer = serializer;
        }

        public EventStream GetEventStream<TEventProvider>(Identity identity)
        {
            int minVersion;
            int maxVersion;

            // get domain events
            var domainEvents = GetDomainEvents(identity, out minVersion, out maxVersion);

            return null;
        }

        public void Commit(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IDomainEvent> GetDomainEvents(Identity identity, out int minVersion, out int maxVersion)
        {
            throw new NotImplementedException();
        }
    }
}
