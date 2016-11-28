using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IDomainEventHandlerFactory _handlerFactory;
        private readonly IDomainEventHandlerContextFactory _contextFactory;

        public DomainEventDispatcher(IDomainEventHandlerFactory handlerFactory,
                                     IDomainEventHandlerContextFactory contextFactory)
        {
            Contract.Requires(handlerFactory != null);
            Contract.Requires(contextFactory != null);

            _handlerFactory = handlerFactory;
            _contextFactory = contextFactory;
        }

        public void Publish(params IDomainEventHandlerContext[] contexts)
        {
            foreach (var context in contexts)
            {
                Contract.Assume(context != null);

                var handlers = GetHandlers(context.DomainEvent);

                HandleDomainEvent(context, handlers);
            }
        }

        public void Publish(params IDomainEvent[] domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                Contract.Assume(domainEvent != null);
                
                var handlers = GetHandlers(domainEvent);
                var context = GetContext(domainEvent);

                HandleDomainEvent(context, handlers);
            }
        }
        
        private IDomainEventHandlerContext GetContext(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);

            return _contextFactory.GetContext(domainEvent);
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

        private static void HandleDomainEvent(IDomainEventHandlerContext context, IDomainEventHandlerCollection handlers)
        {
            Contract.Requires(handlers != null);
            Contract.Requires(context != null);

            var exceptions = new Collection<Exception>();

            foreach(var handler in handlers)
            {
                Contract.Assume(handler != null);

                try
                {
                    HandleDomainEvent(context, handler);
                }
                catch (DomainEventHandlingException exception)
                {
                    exceptions.Add(exception);
                }
            }

            if (exceptions.Count > 1)
            {
                // re-throw exceptions as an aggregate
                throw new AggregateException("One or more domain event handling exceptions occurred.", exceptions);
            }
        }

        private static void HandleDomainEvent(IDomainEventHandlerContext context, IDomainEventHandler handler)
        {
            Contract.Requires(handler != null);
            Contract.Requires(context != null);

            try
            {
                // handle command
                handler.Handle(context);
            }
            catch (Exception e)
            {
                // re-throw exception as a domain event handling exception
                throw new DomainEventHandlingException(context.DomainEvent, e, "Exception occurred in domain event handler, check inner exception for details.");
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlerFactory != null);
            Contract.Invariant(_contextFactory != null);
        }
    }
}
