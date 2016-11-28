using DotNetRevolution.Core.Base;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStoreTransactionAnnouncerContract))]
    public abstract class EventStoreTransactionAnnouncer : Disposable
    {
        private readonly IEventStore _eventStore;

        protected EventStoreTransactionAnnouncer(IEventStore eventStore)
        {
            Contract.Requires(eventStore != null);

            _eventStore = eventStore;
            _eventStore.TransactionCommitted += EventStoreTransactionCommitted;
        }

        protected abstract void TransactionCommitted(EventProviderTransaction transaction);

        private void EventStoreTransactionCommitted(object sender, TransactionCommittedEventArgs e)
        {
            TransactionCommitted(e.Transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventStore.TransactionCommitted -= EventStoreTransactionCommitted;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStore != null);
        }
    }
}
