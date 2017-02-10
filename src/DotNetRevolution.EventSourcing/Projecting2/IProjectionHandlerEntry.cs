using System;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    public interface IProjectionHandlerEntry
    {
        Type DomainEventType { get; }

        Type ProjectionHandlerType { get; }
    }
}
