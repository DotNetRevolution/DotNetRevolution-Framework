using System.Diagnostics.Contracts;
using DotNetRevolution.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionEventType
    {
        public int TransactionEventTypeId { get; private set; }

        public string FullName { get; private set; }

        [UsedImplicitly]
        private TransactionEventType()
        {
        }

        public TransactionEventType(string fullName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(fullName), "fullName");

            FullName = fullName;
        }
    }
}