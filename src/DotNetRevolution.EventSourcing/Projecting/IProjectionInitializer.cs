using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionInitializerContract))]
    public interface IProjectionInitializer
    {
        void Initialize<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;

        void Initialize<TAggregateRoot>(int batchSize) where TAggregateRoot : class, IAggregateRoot;

        Task InitializeAsync<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;

        Task InitializeAsync<TAggregateRoot>(int batchSize) where TAggregateRoot : class, IAggregateRoot;
    }
}
