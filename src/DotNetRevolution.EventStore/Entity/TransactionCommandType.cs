using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionCommandType
    {
        public int TransactionCommandTypeId { get; }

        public string FullName { get; }

        [UsedImplicitly]
        private TransactionCommandType()
        {
        }

        public TransactionCommandType(string fullName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(fullName));

            FullName = fullName;
        }
    }
}