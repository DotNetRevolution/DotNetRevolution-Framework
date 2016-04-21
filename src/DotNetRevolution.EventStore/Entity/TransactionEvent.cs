using DotNetRevolution.Core.Base;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionEvent
    {
        public long TransactionEventId { get; }
        public long TransactionEventProviderId { get; }
        public int TransactionEventTypeId { get; }
        public int Sequence { get; }
        public string Data { get; }

        public virtual TransactionEventType TransactionEventType { get; }
        public virtual TransactionEventProvider TransactionEventProvider { get; }

        [UsedImplicitly]
        private TransactionEvent()
        {
        }

        internal TransactionEvent(TransactionEventProvider transactionEventProvider, int sequence, TransactionEventType transactionEventType, string data)
            : this()
        {
            Contract.Requires(transactionEventProvider != null);
            Contract.Requires(transactionEventType != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(data));

            TransactionEventProvider = transactionEventProvider;
            Sequence = sequence;
            Data = data;
            TransactionEventType = transactionEventType;
        }
    }
}