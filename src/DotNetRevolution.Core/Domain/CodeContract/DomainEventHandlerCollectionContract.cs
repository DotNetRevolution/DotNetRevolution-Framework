using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandlerCollection))]
    internal abstract class DomainEventHandlerCollectionContract : IDomainEventHandlerCollection
    {
        public Type DomainEventType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

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
