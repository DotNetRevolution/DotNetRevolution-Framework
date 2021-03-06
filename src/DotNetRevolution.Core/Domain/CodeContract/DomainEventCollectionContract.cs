﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventCollection))]
    internal abstract class DomainEventCollectionContract : IDomainEventCollection
    {
        public IAggregateRoot AggregateRoot
        {
            get
            {
                Contract.Ensures(Contract.Result<IAggregateRoot>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract int Count { get; }

        public abstract IEnumerator<IDomainEvent> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
