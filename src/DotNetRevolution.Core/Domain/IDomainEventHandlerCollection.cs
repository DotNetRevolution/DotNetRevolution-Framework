using DotNetRevolution.Core.Domain.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventHandlerCollectionContract))]
    public interface IDomainEventHandlerCollection : IReadOnlyCollection<IDomainEventHandler>
    {
        object DomainEvent { [Pure] get; }
    }
}
