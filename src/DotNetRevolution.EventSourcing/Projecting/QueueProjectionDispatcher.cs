using DotNetRevolution.Core.Base;
using System.Diagnostics.Contracts;

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

            _projectionDispatcher.Dispatch(item.Items);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionDispatcher != null);
        }
    }
}
