using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionInitializerContract<>))]
    public interface IProjectionInitializer<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        void Initialize();

        void Initialize(int batchSize);

        Task InitializeAsync();

        Task InitializeAsync(int batchSize);
    }
}
