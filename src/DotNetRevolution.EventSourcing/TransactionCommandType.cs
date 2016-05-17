using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class TransactionCommandType : ValueObject<TransactionCommandType>
    {
        public string FullName { get; }

        public TransactionCommandType(Type transactionCommandType)
        {
            Contract.Requires(transactionCommandType != null);

            FullName = transactionCommandType.FullName;
        }
    }
}
