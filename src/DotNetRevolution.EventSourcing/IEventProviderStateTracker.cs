using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventProviderStateTrackerContract))]
    public interface IEventProviderStateTracker : IStateTracker
    {
        [Pure]
        IReadOnlyCollection<EventStreamRevision> Revisions { get; }

        IEventProvider EventProvider { get; }

        EventProviderVersion LatestVersion { get; }

        void Commit();
    }
}