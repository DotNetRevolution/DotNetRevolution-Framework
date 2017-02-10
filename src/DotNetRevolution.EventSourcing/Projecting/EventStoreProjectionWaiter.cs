using DotNetRevolution.Core.Commanding;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class EventStoreProjectionWaiter : IProjectionWaiter
    {
        private const string ProjectionTimeoutMessage = "Projection did not process transaction before the timeout period elapsed";

        private readonly IProjectionManager _projectionManager;

        public EventStoreProjectionWaiter(IProjectionManager projectionManager)
        {
            Contract.Requires(projectionManager != null);

            _projectionManager = projectionManager;
        }
        
        public void Wait(ICommandHandlingResult result)
        {
            Wait(result, TimeSpan.MaxValue);
        }

        public void Wait(ICommandHandlingResult result, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            var eventStoreCommandHandlingResult = result as EventStoreCommandHandlingResult;
            Contract.Assume(eventStoreCommandHandlingResult != null);

            while (_projectionManager.ProcessedTransactions.FirstOrDefault(x => x == eventStoreCommandHandlingResult.TransactionIdentity) == null)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException(ProjectionTimeoutMessage);
                }

                Thread.Sleep(10);
            }
        }

        public Task WaitAsync(ICommandHandlingResult result)
        {            
            return WaitAsync(result, TimeSpan.MaxValue);
        }

        public async Task WaitAsync(ICommandHandlingResult result, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            var eventStoreCommandHandlingResult = result as EventStoreCommandHandlingResult;
            Contract.Assume(eventStoreCommandHandlingResult != null);

            while (_projectionManager.ProcessedTransactions.FirstOrDefault(x => x == eventStoreCommandHandlingResult.TransactionIdentity) == null)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException(ProjectionTimeoutMessage);
                }

                await Task.Delay(10);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionManager != null);
        }
    }
}
