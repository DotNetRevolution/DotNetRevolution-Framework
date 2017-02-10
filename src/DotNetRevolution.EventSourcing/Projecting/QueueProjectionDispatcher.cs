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
            Queue.Add(new QueueItem<EventProviderTransaction>(eventProviderTransaction));
        }

        public void Dispatch(params EventProviderTransaction[] eventProviderTransactions)
        {
            Queue.Add(new QueueItem<EventProviderTransaction>(eventProviderTransactions));
        }

        protected override void Dispatch(QueueItem queueItem)
        {
            _projectionDispatcher.Dispatch(((QueueItem<EventProviderTransaction>)queueItem).Items);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionDispatcher != null);
        }
    }
}
