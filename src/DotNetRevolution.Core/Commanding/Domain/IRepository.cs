using DotNetRevolution.Core.Commanding.Domain.CodeContract;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.Domain
{
    [ContractClass(typeof(RepositoryContract<>))]
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        TAggregateRoot GetByIdentity(AggregateRootIdentity identity);

        Task<TAggregateRoot> GetByIdentityAsync(AggregateRootIdentity identity);

        ICommandHandlingResult Commit(ICommandHandlerContext context, TAggregateRoot aggregateRoot);

        Task<ICommandHandlingResult> CommitAsync(ICommandHandlerContext context, TAggregateRoot aggregateRoot);
    }
}
