using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventDispatcherContract))]
    public interface IDomainEventDispatcher
    {
        void Publish(IDomainEvent domainEvent);

        void PublishAll(IEnumerable<IDomainEvent> domainEvents);
    }
}
