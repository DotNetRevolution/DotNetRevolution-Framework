namespace DotNetRevolution.EventStore.Command.Transaction
{
    public class AnnounceCommand
    {
        public long TransactionId { get; }

        public AnnounceCommand(long transactionId)
        {
            TransactionId = transactionId;
        }
    }
}