using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class QueueProjectionDispatcher : QueueDispatcher, IProjectionDispatcher
    {
        private readonly IProjectionDispatcher _projectionDispatcher;

        public QueueProjectionDispatcher(IProjectionDispatcher projectionDispatcher)
        {
            Contract.Requires(projectionDispatcher != null);

            _projectionDispatcher = projectionDispatcher;
        }

        public bool Processed(TransactionIdentity transactionIdentity)
        {
            return _projectionDispatcher.Processed(transactionIdentity);
        }

        public void Dispatch(EventProviderTransaction eventProviderTransaction)
        {
            Contract.Assume(Queue != null);

            Queue.Add(new QueueItem<EventProviderTransaction>(eventProviderTransaction));
        }

        public void Dispatch(params EventProviderTransaction[] eventProviderTransactions)
        {
            Contract.Assume(Queue != null);

            Queue.Add(new QueueItem<EventProviderTransaction>(eventProviderTransactions));
        }

        public void Wait()
        {
            Contract.Assume(Queue != null);

            while (Queue.Count > 0)
            {
                Thread.Sleep(10);
            }
        }

        protected override void Dispatch(QueueItem queueItem)
        {
            var item = (QueueItem<EventProviderTransaction>)queueItem;
            Contract.Assume(item?.Items != null);
            
            // call projection dispatcher to dispatch items
            _projectionDispatcher.Dispatch(item.Items);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionDispatcher != null);
        }
    }
}
