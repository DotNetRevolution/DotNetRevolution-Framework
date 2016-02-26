using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionAnnouncementError
    {
        public long TransactionAnnouncementErrorId { get; }
        public long TransactionAnnouncementId { get; }
        public long TransactionEventId { get; }
        public string ErrorType { get; }
        public string ErrorMessage { get; }

        public virtual TransactionAnnouncement TransactionAnnouncement { get; }
        public virtual TransactionEvent TransactionEvent { get; }

        [UsedImplicitly]
        private TransactionAnnouncementError()
        {
        }

        internal TransactionAnnouncementError(TransactionAnnouncement transactionAnnouncement, TransactionEvent transactionEvent, string errorType, string errorMessage)
        {
            Contract.Requires(transactionAnnouncement != null);
            Contract.Requires(transactionEvent != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(errorType));
            Contract.Requires(!string.IsNullOrWhiteSpace(errorMessage));

            TransactionAnnouncement = transactionAnnouncement;
            TransactionEvent = transactionEvent;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
        }
    }
}