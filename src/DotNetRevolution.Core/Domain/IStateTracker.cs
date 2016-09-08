using System.Collections.Generic;

namespace DotNetRevolution.Core.Domain
{
    public interface IStateTracker
    {
        void Apply(IDomainEvent domainEvent);

        void Apply(IReadOnlyCollection<IDomainEvent> domainEvents);
    }
}
