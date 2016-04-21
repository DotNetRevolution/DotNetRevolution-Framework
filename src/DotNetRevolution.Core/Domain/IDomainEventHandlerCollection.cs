using DotNetRevolution.Core.Domain.CodeContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventHandlerCollectionContract))]
    public interface IDomainEventHandlerCollection : IReadOnlyCollection<IDomainEventHandler>
    {
        Type DomainEventType { [Pure] get; }
    }
}
