using System;
using System.Diagnostics.Contracts;
namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(EventStoreTransactionAnnouncer))]
    internal abstract class EventStoreTransactionAnnouncerContract : EventStoreTransactionAnnouncer
    {
        public EventStoreTransactionAnnouncerContract(IEventStore eventStore)
            : base(eventStore)
        {
            Contract.Requires(eventStore != null);
        }

        protected override void TransactionCommitted(EventProviderTransaction transaction)
        {
            Contract.Requires(transaction != null);

            throw new NotImplementedException();
        }
    }
}
