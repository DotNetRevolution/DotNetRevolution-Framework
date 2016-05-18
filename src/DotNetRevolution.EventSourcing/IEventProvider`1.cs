using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{    
    [ContractClass(typeof(EventProviderContract<>))]
    public interface IEventProvider<TAggregateRoot> : IEventProvider
        where TAggregateRoot : class
    {
        [Pure]
        TAggregateRoot CreateAggregateRoot();

        [Pure]
        EventProvider<TAggregateRoot> CreateNewVersion(IDomainEventCollection domainEvents);
    }
}