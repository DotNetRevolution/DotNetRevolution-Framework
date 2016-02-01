namespace DotNetRevolution.EventStore.Command.Transaction
{
    public class AnnounceCommand
    {
        public long TransactionId { get; private set; }

        public AnnounceCommand(long transactionId)
        {
            TransactionId = transactionId;
        }
    }
}