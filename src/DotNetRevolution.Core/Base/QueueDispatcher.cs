using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Base
{
    public abstract class QueueDispatcher : Disposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly Task _worker;

        protected BlockingCollection<QueueItem> Queue { get; }

        protected QueueDispatcher()
        {
            Queue = new BlockingCollection<QueueItem>();

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            _worker = new Task(new Action(DoWork), _cancellationToken, TaskCreationOptions.LongRunning);
            _worker.Start();
        }

        protected abstract void Dispatch(QueueItem queueItem);

        protected void CheckForCancellation()
        {
            _cancellationToken.ThrowIfCancellationRequested();
        }

        private void DoWork()
        {
            try
            {
                while (_cancellationToken.IsCancellationRequested == false)
                {
                    // wait for next queue item
                    var queueItem = GetNextQueueItem();

                    // dispatch item
                    Dispatch(queueItem);
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
            var queueItem = Queue.Take(_cancellationToken);
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
            Contract.Invariant(Queue != null);
        }

        protected class QueueItem
        {
            private readonly object[] _items;

            [Pure]
            public object[] Items
            {
                get
                {
                    Contract.Ensures(Contract.Result<object[]>() != null);

                    return _items;
                }
            }

            public QueueItem(params object[] items)
            {
                Contract.Requires(items != null);

                _items = items;
            }

            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_items != null);
            }
        }

        protected class QueueItem<T> : QueueItem
        {
            private readonly T[] _items;

            [Pure]
            public new T[] Items
            {
                get
                {
                    Contract.Ensures(Contract.Result<T[]>() != null);

                    return _items;
                }
            }

            public QueueItem(params T[] items)
                : base(items)
            {
                Contract.Requires(items != null);

                _items = items;
            }

            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_items != null);
            }
        }
    }
}
