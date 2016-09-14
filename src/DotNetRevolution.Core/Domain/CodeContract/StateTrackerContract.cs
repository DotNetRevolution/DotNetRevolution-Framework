using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IStateTracker))]
    internal abstract class StateTrackerContract : IStateTracker
    {
        public void Apply(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            throw new NotImplementedException();
        }

        public void Apply(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);

            throw new NotImplementedException();
        }
    }
}
