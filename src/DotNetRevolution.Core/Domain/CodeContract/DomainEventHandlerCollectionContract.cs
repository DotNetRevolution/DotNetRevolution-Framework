using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandlerCollection))]
    public abstract class DomainEventHandlerCollectionContract : IDomainEventHandlerCollection
    {
        public object DomainEvent
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract int Count { get; }

        public abstract IEnumerator<IDomainEventHandler> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
