using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventProviderStateTracker))]
    internal abstract class EventProviderStateTrackerContract : IEventProviderStateTracker
    {
        public IEventProvider EventProvider
        {
            get
            {
                Contract.Ensures(Contract.Result<IEventProvider>() != null);

                throw new NotImplementedException();
            }
        }

        public EventProviderVersion LatestVersion
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

                throw new NotImplementedException();
            }
        }

        public IReadOnlyCollection<EventStreamRevision> Revisions
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<EventStreamRevision>>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract void Apply(IReadOnlyCollection<IDomainEvent> domainEvents);

        public abstract void Apply(IDomainEvent domainEvent);

        public abstract void Commit();
    }
}
