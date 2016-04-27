using DotNetRevolution.Core.Domain;
using System;
using System.Collections.Generic;

namespace DotNetRevolution.EventStore.Repository
{
    public class EventStoreRepository
    {
        public IReadOnlyCollection<object> GetEventsFor<TEventProviderType>(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(object aggregateRoot)
        {
            throw new NotImplementedException();
        }
    }
}
