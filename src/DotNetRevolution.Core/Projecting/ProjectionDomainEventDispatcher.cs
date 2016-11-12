using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DotNetRevolution.Core.Base;
using System;

namespace DotNetRevolution.Core.Projecting
{
    public abstract class ProjectionDomainEventDispatcher : Disposable, IDomainEventDispatcher
    {
        private readonly IProjectionManagerFactory _projectionManagerFactory;
        private readonly BlockingCollection<ProjectionQueueItem> _queue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly Task _worker;
        
        public ProjectionDomainEventDispatcher(IProjectionManagerFactory projectionManagerFactory)
        {
            Contract.Requires(projectionManagerFactory != null);

            _projectionManagerFactory = projectionManagerFactory;

            _queue = new BlockingCollection<ProjectionQueueItem>();            

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            _worker = new Task(new Action(DoWork), _cancellationToken, TaskCreationOptions.LongRunning);
            _worker.Start();
        }
        
        public void Publish(IDomainEvent domainEvent)
        {
            _queue.Add(new ProjectionQueueItem(domainEvent));                        
        }        

        public void PublishAll(IEnumerable<IDomainEvent> domainEvents)
        {
            _queue.Add(new ProjectionQueueItem(domainEvents));
        }        

        private void DoWork()
        {
            while (true)
            {
                // wait for next queue item
                var projectionQueueItem = GetNextQueueItem();
                
                // got a queue item, get managers
                var projectionManagers = _projectionManagerFactory.GetManagers();
                
                // loop through managers and project domain events
                foreach (var projectionManager in projectionManagers)
                {
                    // leave loop if cancellation requested
                    _cancellationToken.ThrowIfCancellationRequested();

                    Contract.Assume(projectionManager != null);

                    // project
                    projectionManager.Project(projectionQueueItem.DomainEvents);
                }
            }
        }

        private ProjectionQueueItem GetNextQueueItem()
        {
            Contract.Ensures(Contract.Result<ProjectionQueueItem>() != null);

            // get an item from the queue, blocking
            var queueItem = _queue.Take(_cancellationToken);
            Contract.Assume(queueItem != null);

            return queueItem;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // notify to cancel background process
                _cancellationTokenSource.Cancel();

                // wait for worker to finish
                if (_worker.Wait(TimeSpan.FromSeconds(10)) == false)
                {
                    // do something for timeout
                }
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_cancellationTokenSource != null);
            Contract.Invariant(_projectionManagerFactory != null);
            Contract.Invariant(_worker != null);
            Contract.Invariant(_queue != null);
        }
    }
}
