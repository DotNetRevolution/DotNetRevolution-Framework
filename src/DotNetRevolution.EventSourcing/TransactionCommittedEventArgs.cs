using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class TransactionCommittedEventArgs : EventArgs
    {
        public TransactionIdentity TransactionIdentity { get; }

        public IEventProvider EventProvider { get; }

        public ICommand Command { get; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        public TransactionCommittedEventArgs(EventProviderTransaction transaction, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(transaction != null);
            Contract.Requires(domainEvents != null);

            TransactionIdentity = transaction.Identity;
            Command = transaction.Command;
            EventProvider = transaction.EventStream.EventProvider;
            DomainEvents = domainEvents;
        }
    }
}
