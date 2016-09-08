using DotNetRevolution.Core.Commanding.CodeContract;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(RepositoryContract<>))]
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        TAggregateRoot GetByIdentity(Identity identity);

        void Commit(ICommand command, TAggregateRoot aggregateRoot);
    }
}
