using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    public interface IProjectionHandlerCatalog
    {
        ProjectionType ProjectionType { get; }

        IReadOnlyCollection<Type> DomainEventTypes { [Pure] get; }

        IProjectionHandlerCatalog Add(IProjectionHandlerEntry entry);

        [Pure]
        IProjectionHandlerEntry GetEntry(Type domainEventType);
    }    
}
