using DotNetRevolution.Core.Domain.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventCollectionContract))]
    public interface IDomainEventCollection : IReadOnlyCollection<object>
    {
        AggregateRoot AggregateRoot { [Pure] get; }
    }
}
