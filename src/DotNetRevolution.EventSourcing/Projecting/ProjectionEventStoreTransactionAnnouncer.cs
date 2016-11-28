using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionEventStoreTransactionAnnouncer : EventStoreTransactionAnnouncer
    {
        private readonly IProjectionManagerFactory _projectionManagerFactory;

        public ProjectionEventStoreTransactionAnnouncer(IEventStore eventStore, IProjectionManagerFactory projectionManagerFactory) 
            : base(eventStore)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(projectionManagerFactory != null);

            _projectionManagerFactory = projectionManagerFactory;
        }

        protected override void TransactionCommitted(EventProviderTransaction transaction)
        {   
            // get managers
            var projectionManagers = _projectionManagerFactory.GetManagers();

            // loop through managers and project domain events
            foreach (var projectionManager in projectionManagers)
            {
                Contract.Assume(projectionManager != null);

                // project
                projectionManager.Project(transaction);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionManagerFactory != null);
        }
    }
}