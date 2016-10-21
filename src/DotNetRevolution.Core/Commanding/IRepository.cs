using DotNetRevolution.Core.Commanding.CodeContract;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(RepositoryContract<>))]
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        TAggregateRoot GetByIdentity(AggregateRootIdentity identity);

        Task<TAggregateRoot> GetByIdentityAsync(AggregateRootIdentity identity);

        ICommandHandlingResult Commit(ICommand command, TAggregateRoot aggregateRoot);

        Task<ICommandHandlingResult> CommitAsync(ICommand command, TAggregateRoot aggregateRoot);
    }
}
