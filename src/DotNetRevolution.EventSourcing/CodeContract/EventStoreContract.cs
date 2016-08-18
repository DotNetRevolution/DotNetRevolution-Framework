using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStore))]
    internal abstract class EventStoreContract : IEventStore
    {
        public void Commit(EventProviderTransaction transaction)
        {
            Contract.Requires(transaction != null);
        }
        
        public IEventStream GetEventStream<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<IEventStream> () != null);

            throw new NotImplementedException();
        }
    }
}
