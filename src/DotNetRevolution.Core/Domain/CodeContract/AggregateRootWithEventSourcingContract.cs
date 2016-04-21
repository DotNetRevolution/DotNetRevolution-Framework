using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootWithEventSourcing))]
    internal abstract class AggregateRootWithEventSourcingContract : IAggregateRootWithEventSourcing
    {
        public abstract Identity Identity { get; }

        public IDomainEventCollection UncommittedEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
