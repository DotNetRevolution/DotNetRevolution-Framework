using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class TransactionEventType : ValueObject<TransactionEventType>
    {
        public string FullName { get; }

        public TransactionEventType(Type eventType)
        {
            Contract.Requires(eventType != null);

            FullName = eventType.FullName;
        }
    }
}
