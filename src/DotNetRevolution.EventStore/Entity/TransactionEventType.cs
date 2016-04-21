using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionEventType
    {
        public int TransactionEventTypeId { get; }

        public string FullName { get; }

        [UsedImplicitly]
        private TransactionEventType()
        {
        }

        internal TransactionEventType(string fullName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(fullName));

            FullName = fullName;
        }
    }
}