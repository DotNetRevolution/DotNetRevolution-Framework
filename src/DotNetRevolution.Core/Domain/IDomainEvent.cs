using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public interface IDomainEvent
    {
        Guid DomainEventId { [Pure] get; }
    }
}
