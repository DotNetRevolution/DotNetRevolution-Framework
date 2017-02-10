using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.EventSourcing.Projecting3
{
    public abstract class EventProviderTransactionDispatcher
    {
        protected EventProviderTransactionDispatcher()
        {

        }

        public abstract void Dispatch(params EventProviderTransaction[] transactions);
    }

    public class ProjectionManagerFactory
    {
        public ProjectionManagerFactory()
        {

        }

        public ProjectionManager<TProjection> GetProjectionManager<TProjection>() where TProjection : Projection
        {
            return null;
        }
    }

    public abstract class ProjectionManager<TProjection>
        where TProjection : Projection
    {
        public abstract void Handle(params EventProviderTransaction[] p);
    }

    public abstract class Projection
    {
        public abstract void Project(EventProviderTransaction transaction);
    }
}


// poller/direct => dispatcher => manager => projection
//                             => persistence (repository)

// poller       - uses an interval to retrieve events from event store and dispatches using dispatcher
//              ? must keep track of what has been delivered
// direct       - listens for changes to the IEventStore and dispatches them via the dispatcher
// poller/direct - catches concurrency exception and retrieves changes that are missing

// dispatcher   - receives collection of revisions and find managers via projection catalog

// projection catalog - collection of projections assigned to projection managers
//                    - one entry per projection type
//                    - returns projection managers that are managing projections interested in specified event

// projection manager factory - creates projection manager 

// manager      - receives revisions
//              - optionally loads projection instance to pass to the projection
//                  - saves projection instance via persistence (repository)
//              - saves projection position
//              - raise concurrency exception if projection instance position does not match new changes - 1

// projection   - receives a projection instance and changes state
//              - must inherit interface to let projection catalog know which domain events it cares about

// projection repository - retrieves projection instance
//                       - saves projection instance                        
