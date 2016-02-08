using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventCollection : IDomainEventCollection
    {
        private readonly IReadOnlyCollection<object> _domainEvents;

        public AggregateRoot AggregateRoot { get; }

        public int Count
        {
            get
            {
                return _domainEvents.Count;
            }
        }

        public DomainEventCollection(AggregateRoot aggregateRoot, IReadOnlyCollection<object> domainEvents)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(domainEvents != null);

            AggregateRoot = aggregateRoot;
            _domainEvents = domainEvents;
        }

        public IEnumerator<object> GetEnumerator()
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
