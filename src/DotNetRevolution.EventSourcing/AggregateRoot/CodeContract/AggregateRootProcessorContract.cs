using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.EventSourcing.Snapshotting;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.AggregateRoot.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootProcessor))]
    internal abstract class AggregateRootProcessorContract : IAggregateRootProcessor
    {
        public TAggregateRoot Process<TAggregateRoot>(Snapshot snapshot) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(snapshot != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            throw new NotImplementedException();
        }

        public TAggregateRoot Process<TAggregateRoot>(EventStream eventStream) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(eventStream != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            throw new NotImplementedException();
        }
    }
}
