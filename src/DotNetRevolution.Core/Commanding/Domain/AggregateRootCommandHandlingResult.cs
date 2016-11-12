using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.Domain
{
    public class AggregateRootCommandHandlingResult : CommandHandlingResult
    {
        public AggregateRootIdentity AggregateRootIdentity { get; }

        public AggregateRootCommandHandlingResult(Guid commandId, AggregateRootIdentity aggregateRootIdentity)
            : base(commandId)
        {
            Contract.Requires(commandId != Guid.Empty);
            Contract.Requires(aggregateRootIdentity != null);

            AggregateRootIdentity = aggregateRootIdentity;
        }
    }
}
