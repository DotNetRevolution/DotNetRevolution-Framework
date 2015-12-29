using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventDispatcherContract))]
    public interface IDomainEventDispatcher
    {
        void Publish(object domainEvent);

        void PublishAll(IEnumerable<object> domainEvents);
    }
}