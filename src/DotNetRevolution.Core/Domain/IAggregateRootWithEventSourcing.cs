using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootWithEventSourcingContract))]
    public interface IAggregateRootWithEventSourcing : IAggregateRoot
    {
        IDomainEventCollection UncommittedEvents { [Pure] get; }
    }
}
