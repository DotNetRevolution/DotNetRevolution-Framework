using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IDomainEventHandlerFactory _handlerFactory;

        public DomainEventDispatcher(IDomainEventHandlerFactory handlerFactory)
        {
            Contract.Requires(handlerFactory != null);

            _handlerFactory = handlerFactory;
        }
        
        public void Publish(IDomainEvent domainEvent)
        {
            var handlers = GetHandlers(domainEvent);
            HandleDomainEvent(domainEvent, handlers);
        }
        
        public void PublishAll(IEnumerable<IDomainEvent> domainEvents)
        {
            // publish events
            foreach (var domainEvent in domainEvents)
            {
                Contract.Assume(domainEvent != null);

                Publish(domainEvent);
            }
        }
        
        private IDomainEventHandlerCollection GetHandlers(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);
            Contract.Ensures(Contract.Result<IDomainEventHandlerCollection>() != null);

            try
            {
                // get handler from factory
                return _handlerFactory.GetHandlers(domainEvent.GetType());
            }
            catch (Exception e)
            {
                // throw exception as a domain event handling exception
                throw new DomainEventHandlingException(domainEvent, e, "Could not get a domain event handler for domain event.");
            }
        }

        private static void HandleDomainEvent(IDomainEvent domainEvent, IDomainEventHandlerCollection handlers)
        {
            Contract.Requires(handlers != null);
            Contract.Requires(domainEvent != null);

            var exceptions = new Collection<Exception>();

            foreach(var handler in handlers)
            {
                Contract.Assume(handler != null);

                try
                {
                    HandleDomainEvent(domainEvent, handler);
                }
                catch (DomainEventHandlingException exception)
                {
                    exceptions.Add(exception);
                }
            }

            if (exceptions.Count > 1)
            {
                // rethrow exceptions as an aggregate
                throw new AggregateException("One or more domain event handling exceptions occurred.", exceptions);
            }
        }

        private static void HandleDomainEvent(IDomainEvent domainEvent, IDomainEventHandler handler)
        {
            Contract.Requires(handler != null);
            Contract.Requires(domainEvent != null);

            try
            {
                // handle command
                handler.Handle(domainEvent);
            }
            catch (Exception e)
            {
                // re-throw exception as a domain event handling exception
                throw new DomainEventHandlingException(domainEvent, e, "Exception occurred in domain event handler, check inner exception for details.");
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlerFactory != null);
        }
    }
}
