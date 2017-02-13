using DotNetRevolution.Core.Base;
using System.Diagnostics.Contracts;
using System.Linq;

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

        protected override void Dispatch(QueueItem queueItem)
        {
            var item = (QueueItem<EventProviderTransaction>)queueItem;
            Contract.Assume(item?.Items != null);
            
            // if only 1 item in list, call single dispatch override for optimizations
            if (item.Items.Count() == 1)
            {
                // get single transaction
                var transaction = item.Items.FirstOrDefault();
                Contract.Assume(transaction != null);

                // dispatch single transaction
                _projectionDispatcher.Dispatch(transaction);
            }
            else
            {
                // multiple transactions, call params dispatch override
                _projectionDispatcher.Dispatch(item.Items);
            }            
        }

        public bool Processed(TransactionIdentity transactionIdentity)
        {
            return _projectionDispatcher.Processed(transactionIdentity);       
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionDispatcher != null);
        }
    }
}
