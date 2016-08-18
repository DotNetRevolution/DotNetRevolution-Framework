using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventProviderRepositoryContract<>))]
    public interface IEventProviderRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        IEventStream GetByIdentity(Identity identity, out TAggregateRoot aggregateRoot);

        void Commit(ICommand command, IEventStream eventStream, TAggregateRoot aggregateRoot);
    }
}
