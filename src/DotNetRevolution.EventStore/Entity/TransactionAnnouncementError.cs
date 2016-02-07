using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionAnnouncementError
    {
        public long TransactionAnnouncementErrorId { get; private set; }
        public long TransactionAnnouncementId { get; private set; }
        public long TransactionEventId { get; private set; }
        public string ErrorType { get; private set; }
        public string ErrorMessage { get; private set; }

        public virtual TransactionAnnouncement TransactionAnnouncement { get; private set; }
        public virtual TransactionEvent TransactionEvent { get; private set; }

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