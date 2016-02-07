using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventHandlerCollection : IDomainEventHandlerCollection
    {
        private readonly IReadOnlyCollection<IDomainEventHandler> _handlers;

        public DomainEventHandlerCollection(object domainEvent)
            : this(domainEvent, new Collection<IDomainEventHandler>())
        {
            Contract.Requires(domainEvent != null);
        }

        public DomainEventHandlerCollection(object domainEvent, IReadOnlyCollection<IDomainEventHandler> handlers)
        {
            Contract.Requires(domainEvent != null);
            Contract.Requires(handlers != null);

            DomainEvent = domainEvent;
            _handlers = handlers;
        }

        public object DomainEvent { get; }

        public int Count
        {
            get
            {
                return _handlers.Count;
            }
        }

        public IEnumerator<IDomainEventHandler> GetEnumerator()
        {
            return _handlers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _handlers.GetEnumerator();
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlers != null);
        }
    }
}
