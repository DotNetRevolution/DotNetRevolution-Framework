﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class ActionDomainEventHandler<TDomainEvent> : DomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        private readonly bool _reusable;
        private readonly Action<TDomainEvent> _action;

        public override bool Reusable
        {
            get
            {
                return _reusable;
            }
        }

        public ActionDomainEventHandler(Action<TDomainEvent> action)
        {
            Contract.Requires(action != null);

            _action = action;
        }
        
        public ActionDomainEventHandler(Action<TDomainEvent> action, bool reusable)
        {
            Contract.Requires(action != null);

            _action = action;
            _reusable = reusable;
        }

        public override void Handle(IDomainEventHandlerContext<TDomainEvent> context)
        {
            _action(context.DomainEvent);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_action != null);
        }
    }
}
