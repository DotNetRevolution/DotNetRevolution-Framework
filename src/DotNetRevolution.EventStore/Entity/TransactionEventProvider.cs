namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionEventProvider
    {
        public long TransactionEventProviderId { get; }
        public long TransactionId { get; }
        public int EventProviderId { get; }
        
        public virtual Transaction Transaction { get; }

        public TransactionEventProvider(Transaction transaction, int eventProviderId)
        {
            Transaction = transaction;
            EventProviderId = eventProviderId;
        }
    }
}
