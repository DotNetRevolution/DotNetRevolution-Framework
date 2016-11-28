using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class TransactionCommittedEventArgs : EventArgs
    {
        private readonly EventProviderTransaction _transaction;

        public EventProviderTransaction Transaction
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<EventProviderTransaction>() != null);

                return _transaction;
            }
        }

        public TransactionCommittedEventArgs(EventProviderTransaction transaction)
        {
            Contract.Requires(transaction != null);

            _transaction = transaction;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_transaction != null);
        }
    }
}
