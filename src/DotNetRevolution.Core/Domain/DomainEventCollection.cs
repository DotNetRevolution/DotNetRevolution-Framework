using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventCollection : IDomainEventCollection
    {
        private readonly IReadOnlyCollection<IDomainEvent> _domainEvents;

        public IAggregateRoot AggregateRoot { get; }

        public int Count
        {
            get
            {
                return _domainEvents.Count;
            }
        }

        public DomainEventCollection(IAggregateRoot aggregateRoot, IDomainEvent domainEvent)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(domainEvent != null);

            AggregateRoot = aggregateRoot;
            _domainEvents = new List<IDomainEvent> { domainEvent }.AsReadOnly();
        }

        public DomainEventCollection(IAggregateRoot aggregateRoot, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(domainEvents != null);

            AggregateRoot = aggregateRoot;
            _domainEvents = domainEvents;
        }

        public DomainEventCollection(IAggregateRoot aggregateRoot, params IDomainEvent[] domainEvents)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(domainEvents != null);

            AggregateRoot = aggregateRoot;
            _domainEvents = domainEvents;
        }

        public IEnumerator<IDomainEvent> GetEnumerator()
        {
            return _domainEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _domainEvents.GetEnumerator();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_domainEvents != null);
        }
    }
}
