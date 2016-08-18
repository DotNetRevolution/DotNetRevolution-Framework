using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventProviderContract))]
    public interface IEventProvider
    {
        Identity GlobalIdentity { [Pure] get; }
        Identity Identity { [Pure] get; }
        EventProviderType EventProviderType { [Pure] get; }
    }
}