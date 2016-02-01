using DotNetRevolution.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionEvent
    {
        public long TransactionEventId { get; private set; }
        public long TransactionId { get; private set; }
        public int TransactionEventTypeId { get; private set; }
        public int Sequence { get; private set; }
        public string Data { get; private set; }

        public virtual TransactionEventType TransactionEventType { get; private set; }
        public virtual Transaction Transaction { get; private set; }

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