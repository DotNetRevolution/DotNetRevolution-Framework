using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class TransactionCommand
    {
        public long TransactionId { get; private set; }
        
        public int TransactionCommandTypeId { get; private set; }

        public virtual TransactionCommandType TransactionCommandType { get; private set; }

        public string Data { get; private set; }

        public virtual Transaction Transaction { get; private set; }

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