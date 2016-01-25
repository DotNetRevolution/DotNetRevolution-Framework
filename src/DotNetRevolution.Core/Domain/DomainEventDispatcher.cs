using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IDomainEventCatalog _catalog;

        public DomainEventDispatcher(IDomainEventCatalog catalog)
        {
            Contract.Requires(catalog != null);

            _catalog = catalog;
        }
        
        public void Publish(object domainEvent)
        {
            HandlePublishEvent(domainEvent);
        }
        
        public void PublishAll(IEnumerable<object> domainEvents)
        {
            // publish events
            foreach (var domainEvent in domainEvents)
            {
                Contract.Assume(domainEvent != null);

                Publish(domainEvent);
            }
        }
        
        private void HandlePublishEvent(object domainEvent)
        {
            Contract.Requires(domainEvent != null);

            // get type of domain event
            var domainEventType = domainEvent.GetType();

            // find entries in catalog based on domain event type
            do
            {
                IReadOnlyCollection<IDomainEventEntry> entries;

                if (!_catalog.TryGetEntries(domainEventType, out entries))
                {
                    // no entries, try base type if any
                    continue;
                }
                
                Contract.Assume(entries != null);

                var exceptions = new List<Exception>();

                // execute handlers
                foreach (var entry in entries)
                {
                    Contract.Assume(entry != null);

                    try
                    {    
                        // get a domain event handler
                        var handler = GetHandler(entry);

                        // handle domain event
                        handler.Handle(domainEvent);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    // re-throw exceptions
                    throw new AggregateException("One or more domain event handling exceptions occurred.", exceptions);
                }

                // while base type is not null or object, attempt to find entries.  this is for backwards compatibility
            } while ((domainEventType = domainEventType.BaseType) != null && domainEventType != typeof(object));
        }

        protected virtual IDomainEventHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            return (IDomainEventHandler) Activator.CreateInstance(handlerType);
        }

        private IDomainEventHandler GetHandler(IDomainEventEntry entry)
        {
            Contract.Requires(entry != null);
            
            // get handler from entry
            var handler = entry.DomainEventHandler;

            // if handler is cached, return handler
            if (handler != null)
            {
                return handler;
            }

            // create handler
            handler = CreateHandler(entry.DomainEventHandlerType);
            Contract.Assume(handler != null);

            // if handler is reusable, cache in entry
            if (handler.Reusable)
            {
                entry.DomainEventHandler = handler;
            }

            return handler;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_catalog != null);
        }
    }
}
