using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventHandlerCollection : IDomainEventHandlerCollection
    {
        private readonly IReadOnlyCollection<IDomainEventHandler> _handlers;

        public DomainEventHandlerCollection(Type domainEventType)
            : this(domainEventType, new Collection<IDomainEventHandler>())
        {
            Contract.Requires(domainEventType != null);
        }

        public DomainEventHandlerCollection(Type domainEventType, IReadOnlyCollection<IDomainEventHandler> handlers)
        {
            Contract.Requires(domainEventType != null);
            Contract.Requires(handlers != null);

            DomainEventType = domainEventType;
            _handlers = handlers;
        }

        public Type DomainEventType { get; }

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
