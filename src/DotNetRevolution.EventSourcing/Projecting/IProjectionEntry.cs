using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionEntryContract))]
    public interface IProjectionEntry
    {
        ProjectionType ProjectionType { get; }

        AggregateRootType AggregateRootType { get; }

        Type ProjectionManagerType { get; }

        IProjectionManager ProjectionManager { get; }
    }
}