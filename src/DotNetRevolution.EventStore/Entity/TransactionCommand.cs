using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionCommand
    {
        public long TransactionId { get; }
        
        public int TransactionCommandTypeId { get; }

        public virtual TransactionCommandType TransactionCommandType { get; }

        public string Data { get; }

        public virtual Transaction Transaction { get; }

        [UsedImplicitly]
        private TransactionCommand()
        {
        }

        internal TransactionCommand(Transaction transaction, TransactionCommandType transactionCommandType, string commandData)
        {
            Contract.Requires(transactionCommandType != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(commandData));

            Transaction = transaction;
            TransactionCommandType = transactionCommandType;
            Data = commandData;
        }
    }
}