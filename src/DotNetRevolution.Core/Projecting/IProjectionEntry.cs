using DotNetRevolution.Core.Projecting.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    [ContractClass(typeof(ProjectionEntryContract))]
    public interface IProjectionEntry
    {
        Type ProjectionType { get; }

        Type ProjectionManagerType { get; }

        IProjectionManager ProjectionManager { get; }
    }
}