using DotNetRevolution.Core.Domain.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(StateTrackerContract))]
    public interface IStateTracker
    {
        void Apply(IDomainEvent domainEvent);

        void Apply(IReadOnlyCollection<IDomainEvent> domainEvents);
    }
}
