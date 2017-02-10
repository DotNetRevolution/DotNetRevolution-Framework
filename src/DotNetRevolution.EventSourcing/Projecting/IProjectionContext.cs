using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Metadata;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionContextContract))]
    public interface IProjectionContext
    {
        [Pure]
        TransactionIdentity TransactionIdentity { get; }

        [Pure]
        IEventProvider EventProvider { get; }

        [Pure]
        ICommand Command { get; }

        [Pure]
        IReadOnlyCollection<Meta> Metadata { get; }

        [Pure]
        EventProviderVersion Version { get; }

        [Pure]
        object Data { get; }
    }
}
