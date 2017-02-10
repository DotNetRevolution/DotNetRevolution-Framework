using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Metadata;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderTransaction
    {
        private readonly TransactionIdentity _identity;
        private readonly ICommand _command;
        private readonly IReadOnlyCollection<EventStreamRevision> _revisions;
        private readonly EventProviderDescriptor _descriptor;
        private readonly IEventProvider _eventProvider;
        private readonly IReadOnlyCollection<Meta> _metadata;

        public TransactionIdentity Identity
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<TransactionIdentity>() != null);

                return _identity;
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

        public IEventProvider EventProvider
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<IEventProvider>() != null);

                return _eventProvider;
            }
        }

        public IReadOnlyCollection<EventStreamRevision> Revisions
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<EventStreamRevision>>() != null);

                return _revisions;
            }
        }

        public EventProviderDescriptor Descriptor
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<EventProviderDescriptor>() != null);

                return _descriptor;
            }
        }

        public IReadOnlyCollection<Meta> Metadata
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<Meta>>() != null);

                return _metadata;
            }
        }

        public EventProviderTransaction(TransactionIdentity identity, IEventProvider eventProvider, ICommand command, IAggregateRoot aggregateRoot, IReadOnlyCollection<EventStreamRevision> revisions, IReadOnlyCollection<Meta> metadata)
        {
            Contract.Requires(command != null);
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(eventProvider != null);
            Contract.Requires(revisions != null);
            Contract.Requires(metadata != null);

            Contract.Assume(identity != null);

            _identity = identity;
            _command = command;
            _eventProvider = eventProvider;
            _revisions = revisions;
            _descriptor = new EventProviderDescriptor(aggregateRoot);
            _metadata = metadata;
        }

        public EventProviderTransaction(TransactionIdentity identity, IEventProvider eventProvider, ICommand command, EventProviderDescriptor descriptor, IReadOnlyCollection<EventStreamRevision> revisions, IReadOnlyCollection<Meta> metadata)
        {
            Contract.Requires(command != null);
            Contract.Requires(descriptor != null);
            Contract.Requires(eventProvider != null);
            Contract.Requires(revisions != null);
            Contract.Requires(metadata != null);

            Contract.Assume(identity != null);

            _identity = identity;
            _command = command;
            _eventProvider = eventProvider;
            _revisions = revisions;
            _descriptor = descriptor;
            _metadata = metadata;
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEvent>>() != null);

            var domainEventRevisions = _revisions.Where(x => x is DomainEventRevision)
                                                 .OrderBy(x => x.Version)
                                                 .Cast<DomainEventRevision>()
                                                 .ToList();

            var domainEvents = domainEventRevisions.Aggregate(new List<IDomainEvent>(), (seed, revision) =>
                {
                    seed.AddRange(revision.DomainEvents);
                    return seed;
                });
            Contract.Assume(domainEvents != null);

            return domainEvents;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identity != null);
            Contract.Invariant(_command != null);
            Contract.Invariant(_revisions != null);
            Contract.Invariant(_eventProvider != null);
            Contract.Invariant(_descriptor != null);
            Contract.Invariant(_metadata != null);
        }
    }
}
