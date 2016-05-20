using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public interface IEventStreamDomainEventCollection : IReadOnlyCollection<IDomainEvent>
    {
        IEventStream EventStream { [Pure] get; }
    }
}
