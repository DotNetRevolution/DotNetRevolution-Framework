using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;

namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRoot
    {
        public static string DomainEventApplyMethodName { get; set; }

        private readonly Type _type;
        private readonly List<object> _uncommittedEvents;

        public abstract Guid AggregateId { get; }
        
        public abstract string AggregateDescription { get; }

        public IDomainEventCollection UncommittedEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

                return new DomainEventCollection(this, _uncommittedEvents);
            }
        }

        static AggregateRoot()
        {
            DomainEventApplyMethodName = "Apply";
        }

        protected AggregateRoot()
        {
            _type = GetType();
            _uncommittedEvents = new List<object>();
        }

        protected AggregateRoot(IReadOnlyCollection<object> events)
            : this()
        {
            Contract.Requires(events != null);
            Contract.Requires(Contract.ForAll(events, o => o != null));

            foreach (var domainEvent in events)
            {
                ApplyDomainEvent(domainEvent);
            }
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = ".Net event model does not work for domain events.")]
        protected void RaiseDomainEvent(object domainEvent)
        {
            _uncommittedEvents.Add(domainEvent);

            ApplyDomainEvent(domainEvent);
        }

        private void ApplyDomainEvent(object domainEvent)
        {
            _type.InvokeMember(DomainEventApplyMethodName,
                               BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                               null,
                               this,
                               new [] { domainEvent }, 
                               CultureInfo.InvariantCulture);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_type != null);
            Contract.Invariant(_uncommittedEvents != null);
        }
    }
}
