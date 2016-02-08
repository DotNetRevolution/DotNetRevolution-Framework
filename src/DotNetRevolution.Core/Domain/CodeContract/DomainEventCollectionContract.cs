using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventCollection))]
    public abstract class DomainEventCollectionContract : IDomainEventCollection
    {
        public AggregateRoot AggregateRoot
        {
            get
            {
                Contract.Ensures(Contract.Result<AggregateRoot>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract int Count { get; }

        public abstract IEnumerator<object> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
