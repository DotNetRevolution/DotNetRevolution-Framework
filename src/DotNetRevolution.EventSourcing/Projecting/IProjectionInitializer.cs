using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionInitializerContract))]
    public interface IProjectionInitializer
    {
        EventProviderTransaction Initialize<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;

        EventProviderTransaction Initialize<TAggregateRoot>(int batchSize) where TAggregateRoot : class, IAggregateRoot;

        Task<EventProviderTransaction> InitializeAsync<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;

        Task<EventProviderTransaction> InitializeAsync<TAggregateRoot>(int batchSize) where TAggregateRoot : class, IAggregateRoot;
    }
}
