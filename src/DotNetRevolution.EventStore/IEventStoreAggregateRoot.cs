using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventStore
{
    public interface IEventStoreAggregateRoot : IAggregateRoot
    {
        IDomainEventCollection UncommittedEvents { [Pure] get; }
        Type AggregateRootType { [Pure] get; }
        string AggregateDescription { [Pure] get; }
    }
}
