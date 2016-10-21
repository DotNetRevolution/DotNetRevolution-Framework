using System;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStoreCommandHandlingResult : AggregateRootCommandHandlingResult
    {
        public TransactionIdentity TransactionIdentity { get; }

        public EventStoreCommandHandlingResult(Guid commandId, AggregateRootIdentity aggregateRootIdentity, TransactionIdentity transactionIdentity) 
            : base(commandId, aggregateRootIdentity)
        {
            Contract.Requires(commandId != Guid.Empty);
            Contract.Requires(aggregateRootIdentity != null);
            Contract.Requires(transactionIdentity != null);

            TransactionIdentity = transactionIdentity;
        }
    }
}
