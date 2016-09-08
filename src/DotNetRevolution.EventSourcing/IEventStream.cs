using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStreamContract))]
    public interface IEventStream : IEnumerable<EventStreamRevision>
    {
        [Pure]
        IEventProvider EventProvider { get; }

        [Pure]
        void Append(IDomainEvent domainEvent);

        [Pure]
        void Append(IReadOnlyCollection<IDomainEvent> domainEvents);

        [Pure]
        IReadOnlyCollection<EventStreamRevision> GetUncommittedRevisions();
    }
}