using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(AbstractProjectionManagerContract))]
    public abstract class ProjectionManager : IProjectionManager
    {
        private const string ProjectionTimeoutMessage = "Projection did not process transaction before the timeout period elapsed";

        private readonly IProjectionFactory _projectionFactory;

        public ProjectionManager(IProjectionFactory projectionFactory)
        {
            Contract.Requires(projectionFactory != null);

            _projectionFactory = projectionFactory;
        }

        protected abstract bool Processed(TransactionIdentity identity);

        protected abstract void PrepareProjection(IProjection projection);

        protected abstract void FinalizeProjection(IProjection projection, EventProviderTransaction transaction);

        protected abstract void FinalizeProjection(IProjection projection, Exception exception);

        protected abstract void SaveProjection(IProjection projection);

        public void Project(EventProviderTransaction transaction)
        {
            // get projection
            var projection = _projectionFactory.GetProjection();

            // prepare projection
            PrepareProjection(projection);

            try
            {
                // project
                foreach (var domainEvent in transaction.GetDomainEvents())
                {
                    Contract.Assume(domainEvent != null);

                    projection.Project(domainEvent);
                }

                SaveProjection(projection);
            }
            catch (Exception e)
            {
                // finalize projection with error
                FinalizeProjection(projection, e);
                return;
            }

            // finalize projection
            FinalizeProjection(projection, transaction);
        }

        #region Wait

        public void Wait(Guid domainEventId)
        {
            Wait(domainEventId, TimeSpan.MaxValue);
        }

        public void Wait(Guid domainEventId, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (Processed(domainEventId) == false)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException(ProjectionTimeoutMessage);
                }

                Thread.Sleep(10);
            }
        }

        public Task WaitAsync(Guid domainEventId)
        {
            return WaitAsync(domainEventId, TimeSpan.MaxValue);
        }

        public async Task WaitAsync(Guid domainEventId, TimeSpan timeout)
        {            
            var stopwatch = Stopwatch.StartNew();

            while (Processed(domainEventId) == false)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException(ProjectionTimeoutMessage);
                }

                await Task.Delay(10);
            }
        }

        #endregion


        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionFactory != null);
        }
    }
}
