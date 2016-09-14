using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStreamStateTracker))]
    internal abstract class EventStreamStateTrackerContract : IEventStreamStateTracker
    {
        public IEventStream EventStream
        {
            get
            {
                Contract.Ensures(Contract.Result<IEventStream>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract void Apply(IReadOnlyCollection<IDomainEvent> domainEvents);

        public abstract void Apply(IDomainEvent domainEvent);
    }
}
