using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(RepositoryContract<>))]
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        TAggregateRoot GetByIdentity(Identity identity);

        Task<TAggregateRoot> GetByIdentityAsync(Identity identity);

        void Commit(TAggregateRoot aggregateRoot);

        Task CommitAsync(TAggregateRoot aggregateRoot);
    }
}
