using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStreamStateTrackerContract))]
    public interface IEventStreamStateTracker : IStateTracker
    {
        IEventStream EventStream { get; }
    }
}