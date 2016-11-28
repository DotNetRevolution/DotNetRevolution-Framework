using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStreamStateTrackerContract))]
    public interface IEventStreamStateTracker : IStateTracker
    {        
        [Pure]
        IReadOnlyCollection<EventStreamRevision> Revisions { get; }

        IEventStream EventStream { get; }

        void Commit();
    }
}