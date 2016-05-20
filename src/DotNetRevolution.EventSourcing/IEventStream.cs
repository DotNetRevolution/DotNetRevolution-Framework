using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using DotNetRevolution.EventSourcing.Snapshotting;

namespace DotNetRevolution.EventSourcing
{
    public interface IEventStream : IEnumerable<IDomainEvent>
    {
        [Pure]
        IEventStreamDomainEventCollection DomainEvents { get; }

        [Pure]
        Snapshot Snapshot { get; }
    }
}