using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventProviderRepository<>))]
    internal abstract class EventProviderRepositoryContract<TAggregateRoot> : IEventProviderRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public void Commit(ICommand command, IEventStream eventStream)
        {
            Contract.Requires(command != null);
            Contract.Requires(eventStream != null);
            Contract.Requires(eventStream.GetUncommittedRevisions() != null);
            Contract.Requires(eventStream.GetUncommittedRevisions().Count > 0);
        }

        public IEventStream GetByIdentity(Identity identity, out TAggregateRoot aggregateRoot)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<IEventStream>() != null);
            Contract.Ensures(Contract.ValueAtReturn(out aggregateRoot) != null);

            throw new NotImplementedException();
        }
    }
}
