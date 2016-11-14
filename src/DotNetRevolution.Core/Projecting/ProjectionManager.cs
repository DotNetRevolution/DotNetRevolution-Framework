using DotNetRevolution.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Projecting
{
    public abstract class ProjectionManager : IProjectionManager
    {
        private const string ProjectionTimeoutMessage = "Projection did not process transaction before the timeout period elapsed";

        private readonly IProjectionFactory _projectionFactory;

        public ProjectionManager(IProjectionFactory projectionFactory)
        {
            Contract.Requires(projectionFactory != null);

            _projectionFactory = projectionFactory;
        }

        protected abstract bool Processed(Guid domainEventId);

        protected abstract void PrepareProjection(IProjection projection);

        protected abstract void FinalizeProjection(IProjection projection, IDomainEvent[] domainEvents);

        protected abstract void FinalizeProjection(IProjection projection, Exception e);

        protected abstract void SaveProjection(IProjection projection);

        public void Project(params IDomainEvent[] domainEvents)
        {
            // get projection
            var projection = _projectionFactory.GetProjection();

            // prepare projection
            PrepareProjection(projection);

            try
            {
                // project
                foreach (var domainEvent in domainEvents)
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
            FinalizeProjection(projection, domainEvents);
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
