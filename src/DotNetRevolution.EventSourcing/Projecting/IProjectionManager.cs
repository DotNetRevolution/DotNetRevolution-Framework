using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionManagerContract))]
    public interface IProjectionManager
    {
        IReadOnlyCollection<TransactionIdentity> ProcessedTransactions { get; }

        void Handle(IEventProvider eventProvider, params IProjectionContext[] projectionContexts);
    }
}