using DotNetRevolution.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    // should manager only take care of position and holding catalog (it uses IProjectionStore)?
    // should dispatcher use IProjectionStore or ask managers for positions?
    // create cache for event streams and have cache raise events for items removed so caches can notify other caches (aggregate root cache to notify steam cache if exception occurs)
    
    public class ProjectionManager2<TProjection> 
    {
        public ProjectionManager2(IProjectionFactory<TProjection> projectionFactory,
                                  IProjectionHandlerCatalog catalog) 
        {

        }

        public override void Publish(params IDomainEventHandlerContext[] contexts)
        {
            // load projection

            // create projection context

            // publish
            base.Publish(contexts);

            // save projection
        }
    }

    public class ProjectionManagerEntry : IDomainEventEntry
    {
        public IDomainEventHandler DomainEventHandler { get; set; }

        public Type DomainEventHandlerType { get; }

        public Type DomainEventType { get; }

        public ProjectionManagerEntry(ProjectionManager2 projectionManager)
        {
            DomainEventHandler = projectionManager;
            DomainEventHandlerType = projectionManager.GetType();
            DomainEventType = typeof(IDomainEvent);
        }
    }


    public abstract class ProjectionManager : IProjectionManager
    {
        private readonly Dictionary<EventProviderIdentity, ProjectionPosition> _projectionPositions = new Dictionary<EventProviderIdentity, ProjectionPosition>();
        private readonly IProjectionHandlerFactory _projectionHandlerFactory;

        public IProjectionHandlerCatalog ProjectionHandlerCatalog { get; }

        protected ProjectionManager(IProjectionHandlerCatalog catalog,
                                 IProjectionHandlerFactory projectionHandlerFactory)
        {
            Contract.Requires(catalog != null);
            Contract.Requires(projectionHandlerFactory != null);
            
            ProjectionHandlerCatalog = catalog;
            _projectionHandlerFactory = projectionHandlerFactory;
        }
        
        protected abstract IProjectionHandlerContext GetContext(EventProvider eventProvider, IDomainEvent domainEvent);
        
        public void Project(EventProvider eventProvider, params EventStreamRevision[] revisions)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(revisions != null);
            Contract.Requires(revisions.Length > 0);

            EventStreamRevision revision = null;

            for (var i = 0; i < revisions.Length; i++)
            {
                revision = revisions[i];

                var domainEventRevision = revision as DomainEventRevision;
                
                if (domainEventRevision == null)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    foreach (var domainEvent in domainEventRevision)
                    {
                        // get handler
                        var handler = GetHandler(ProjectionHandlerCatalog.GetEntry(domainEvent.GetType()));

                        // create context
                        var context = GetContext(eventProvider, domainEvent);

                        // handle
                        Handle(handler, context);
                    }
                }
            }

            _projectionPositions[eventProvider.Identity] = new ProjectionPosition(revision.Version);
        }

        private void Handle(IProjectionHandler handler, IProjectionHandlerContext context)
        {
            Contract.Requires(handler != null);
            Contract.Requires(context != null);

            try
            {
                handler.Handle(context);
            }
            catch
            {
                throw new ProjectionHandlingException();
            }
        }        

        private IProjectionHandler GetHandler(IProjectionHandlerEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(Contract.Result<IProjectionHandler>() != null);

            return _projectionHandlerFactory.GetHandler(entry);
        }
    }

    public interface IProjectionManager
    {
        IProjectionHandlerCatalog ProjectionHandlerCatalog { get; }
    }

    public interface IProjectionManager<TProjection> : IProjectionManager
        //where TProjection : IProjection
    {
    }
}
