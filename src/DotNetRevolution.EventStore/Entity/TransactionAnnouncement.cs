using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionAnnouncement
    {
        private readonly ICollection<TransactionAnnouncementError> _transactionAnnouncementErrors;

        public long TransactionAnnouncementId { get; }
        public long TransactionId { get; }
        public DateTime Announced { get; }

        public virtual Transaction Transaction { get; }

        public virtual EntityCollection<TransactionAnnouncementError> Errors
        {
            get { return _transactionAnnouncementErrors as EntityCollection<TransactionAnnouncementError>; }
        }
        
        private TransactionAnnouncement()
        {
            _transactionAnnouncementErrors = new EntityCollection<TransactionAnnouncementError>();
        }

        internal TransactionAnnouncement(Transaction transaction)
            : this()
        {
            Contract.Requires(transaction != null);
            
            Transaction = transaction;
            Announced = DateTime.Now;
        }

        internal void RecordError(TransactionEvent transactionEvent, string errorType, string errorMessage)
        {
            Contract.Requires(transactionEvent != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(errorType));
            Contract.Requires(!string.IsNullOrWhiteSpace(errorMessage));

            _transactionAnnouncementErrors.Add(new TransactionAnnouncementError(this, transactionEvent, errorType, errorMessage));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_transactionAnnouncementErrors != null);
        }
    }
}