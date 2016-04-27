using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventEntry : IDomainEventEntry
    {
        public Type DomainEventType { get; }

        public Type DomainEventHandlerType { get; }

        public IDomainEventHandler DomainEventHandler { get; set; }

        public DomainEventEntry(Type domainEventType, Type handlerType)
        {
            Contract.Requires(domainEventType != null);
            Contract.Requires(handlerType != null);

            DomainEventType = domainEventType;
            DomainEventHandlerType = handlerType;
        }

        public DomainEventEntry(Type domainEventType, IDomainEventHandler handler)
        {
            Contract.Requires(domainEventType != null);
            Contract.Requires(handler != null);

            DomainEventType = domainEventType;
            DomainEventHandler = handler;
            DomainEventHandlerType = handler.GetType();
        }

        public static DomainEventEntry New<TDomainEvent>(Action<TDomainEvent> action) where TDomainEvent : IDomainEvent
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<DomainEventEntry>() != null);

            return New(action, false);
        }

        public static DomainEventEntry New<TDomainEvent>(Action<TDomainEvent> action, bool reusable) where TDomainEvent : IDomainEvent
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<DomainEventEntry>() != null);

            return new DomainEventEntry(typeof(TDomainEvent), new ActionDomainEventHandler<TDomainEvent>(action));
        }
    }
}
