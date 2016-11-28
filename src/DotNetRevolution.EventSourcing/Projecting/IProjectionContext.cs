using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Metadata;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public interface IProjectionContext
    {
        [Pure]
        IDomainEvent DomainEvent { get; }

        [Pure]
        IReadOnlyCollection<Meta> Metadata { get; }
    }
}
