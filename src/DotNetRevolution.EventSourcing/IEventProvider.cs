using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventProviderContract))]
    public interface IEventProvider
    {
        EventProviderIdentity EventProviderIdentity { [Pure] get; }

        AggregateRootType AggregateRootType { [Pure] get; }

        AggregateRootIdentity AggregateRootIdentity { [Pure] get; }
    }
}