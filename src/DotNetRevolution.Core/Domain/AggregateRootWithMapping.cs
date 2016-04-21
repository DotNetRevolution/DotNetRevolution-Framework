using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRootWithMapping : AggregateRootWithEventSourcing
    {
        private readonly IDictionary<Type, IDomainEventMethodInvoker> _applyDomainEventMappings = new Dictionary<Type, IDomainEventMethodInvoker>();
        
        public AggregateRootWithMapping()
            : base()
        {
        }

        protected AggregateRootWithMapping(IReadOnlyCollection<object> events) 
            : base(events)
        {
            Contract.Requires(events != null);
        }
        
        protected override void ApplyDomainEvent(object domainEvent)
        {
            Contract.Assume(domainEvent != null);

            IDomainEventMethodInvoker invoker;
                
            if (_applyDomainEventMappings.TryGetValue(domainEvent.GetType(), out invoker))
            {
                Contract.Assume(invoker != null);

                invoker.Invoke(domainEvent);
            }
            else
            {
                throw new MissingApplyDomainEventMappingException(GetType(), domainEvent.GetType());
            }
        }

        protected void AddApplyDomainEventMapping(Type domainEventType, IDomainEventMethodInvoker invoker)
        {
            Contract.Requires(domainEventType != null);
            Contract.Requires(invoker != null);
            Contract.Ensures(_applyDomainEventMappings[domainEventType] != null);

            _applyDomainEventMappings[domainEventType] = invoker;
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_applyDomainEventMappings != null);
        }
    }
}
