using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class TransactionCommittedEventArgs : EventArgs
    {
        private readonly TransactionIdentity _transactionIdentity;
        private readonly IEventProvider _eventProvider;
        private readonly ICommand _command;
        private readonly IReadOnlyCollection<IDomainEvent> _domainEvents;

        public TransactionIdentity TransactionIdentity
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<TransactionIdentity>() != null);

                return _transactionIdentity;
            }
        }

        public IEventProvider EventProvider
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<IEventProvider>() != null);

                return _eventProvider;
            }
        }

        public ICommand Command
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                return _command;
            }
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEvent>>() != null);

                return _domainEvents;
            }
        }

        public TransactionCommittedEventArgs(EventProviderTransaction transaction, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(transaction != null);
            Contract.Requires(domainEvents != null);

            _transactionIdentity = transaction.Identity;
            _command = transaction.Command;
            _eventProvider = transaction.EventStream.EventProvider;
            _domainEvents = domainEvents;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_transactionIdentity != null);
            Contract.Invariant(_command != null);
            Contract.Invariant(_eventProvider != null);
            Contract.Invariant(_domainEvents != null);
        }
    }
}
