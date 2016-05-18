﻿using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStore))]
    internal abstract class EventStoreContract : IEventStore
    {
        public void Commit(Transaction transaction)
        {
            Contract.Requires(transaction != null);
        }

        public EventProvider<TAggregateRoot> GetEventProvider<TAggregateRoot>(Identity identity) where TAggregateRoot : class
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<EventProvider<TAggregateRoot>>() != null);

            throw new NotImplementedException();
        }
    }
}
