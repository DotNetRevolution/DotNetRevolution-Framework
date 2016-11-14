using DotNetRevolution.Core.Base;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain
{
    public class QueueDomainEventDispatcher : Disposable, IDomainEventDispatcher
    {
        private readonly BlockingCollection<QueueItem> _queue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly Task _worker;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public QueueDomainEventDispatcher(IDomainEventDispatcher domainEventDispatcher)
        {
            Contract.Requires(domainEventDispatcher != null);

            _domainEventDispatcher = domainEventDispatcher;

            _queue = new BlockingCollection<QueueItem>();

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            _worker = new Task(new Action(DoWork), _cancellationToken, TaskCreationOptions.LongRunning);                       
            _worker.Start();
        }
        
        public void Publish(params IDomainEvent[] domainEvents)
        {
            _queue.Add(new QueueItem(domainEvents));
        }        

        protected void CheckForCancellation()
        {
            _cancellationToken.ThrowIfCancellationRequested();
        }

        private void DoWork()
        {
            try
            {
                while (true)
                {
                    // wait for next queue item
                    var queueItem = GetNextQueueItem();

                    _domainEventDispatcher.Publish(queueItem.DomainEvents);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private QueueItem GetNextQueueItem()
        {
            Contract.Ensures(Contract.Result<QueueItem>() != null);

            // get an item from the queue, blocking
            var queueItem = _queue.Take(_cancellationToken);
            Contract.Assume(queueItem != null);

            return queueItem;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // tell task to cancel
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
            Contract.Invariant(_worker != null);
            Contract.Invariant(_queue != null);
            Contract.Invariant(_domainEventDispatcher != null);
        }

        protected class QueueItem
        {
            private readonly IDomainEvent[] _domainEvents;

            [Pure]
            public IDomainEvent[] DomainEvents
            {
                get
                {
                    Contract.Ensures(Contract.Result<IDomainEvent[]>() != null);

                    return _domainEvents;
                }
            }

            public QueueItem(params IDomainEvent[] domainEvents)
            {
                Contract.Requires(domainEvents != null);

                _domainEvents = domainEvents;
            }

            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_domainEvents != null);
            }
        }
    }
}
