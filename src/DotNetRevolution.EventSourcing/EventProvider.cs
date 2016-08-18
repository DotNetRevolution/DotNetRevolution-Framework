﻿using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider : IEventProvider
    {
        public Identity GlobalIdentity { get; }

        public EventProviderType EventProviderType { get; }

        public Identity Identity { get; }        
        
        public EventProvider(Identity globalIdentity,
            EventProviderType type,
            Identity identity)
        {            
            Contract.Requires(type != null);

            Contract.Assume(identity != null);
            Contract.Assume(globalIdentity != null);

            GlobalIdentity = globalIdentity;
            EventProviderType = type;
            Identity = identity;
        }

        public EventProvider(IDomainEventCollection domainEventCollection)
            : this(Identity.New(),
                   new EventProviderType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity)
        {
            Contract.Requires(domainEventCollection?.AggregateRoot != null);
        }
    }
}
