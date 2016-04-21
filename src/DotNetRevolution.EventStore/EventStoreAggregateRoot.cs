using DotNetRevolution.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;

namespace DotNetRevolution.EventStore
{
    public abstract class EventStoreAggregateRoot : AggregateRootWithEventSourcing, IEventStoreAggregateRoot
    {
        public static string DomainEventApplyMethodName { get; set; }

        public abstract string AggregateDescription { get; }

        public Type AggregateRootType { get; private set; }
        
        static EventStoreAggregateRoot()
        {
            DomainEventApplyMethodName = "Apply";
        }

        protected EventStoreAggregateRoot()
            : base()
        {
            AggregateRootType = GetType();
        }

        protected EventStoreAggregateRoot(IReadOnlyCollection<object> events)
            : this()
        {
            Contract.Requires(events != null);
            Contract.Requires(Contract.ForAll(events, o => o != null));

            foreach (var domainEvent in events)
            {
                ApplyDomainEvent(domainEvent);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = ".Net event model does not work for domain events.")]
        protected override void RaiseDomainEvent(object domainEvent)
        {
            base.RaiseDomainEvent(domainEvent);

            ApplyDomainEvent(domainEvent);
        }

        protected override void ApplyDomainEvent(object domainEvent)
        {
            AggregateRootType.InvokeMember(DomainEventApplyMethodName,
                               BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                               null,
                               this,
                               new[] { domainEvent },
                               CultureInfo.InvariantCulture);
        }
    }
}
