using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionEvent
    {
        public long TransactionEventId { get; }
        public long TransactionId { get; }
        public int TransactionEventTypeId { get; }
        public int Sequence { get; }
        public string Data { get; }

        public virtual TransactionEventType TransactionEventType { get; }
        public virtual Transaction Transaction { get; }

        [UsedImplicitly]
        private TransactionEvent()
        {
        }

        internal TransactionEvent(Transaction transaction, int sequence, TransactionEventType transactionEventType, string data)
            : this()
        {
            Transaction = transaction;
            Sequence = sequence;
            Data = data;
            TransactionEventType = transactionEventType;
        }
    }
}