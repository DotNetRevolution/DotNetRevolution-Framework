using DotNetRevolution.Core.Commanding;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class EventStoreProjectionWaiter : IProjectionWaiter
    {
        private const string ProjectionTimeoutMessage = "Transaction not processed before the timeout period elapsed";

        private readonly IProjectionDispatcher _projectionDispatcher;

        public EventStoreProjectionWaiter(IProjectionDispatcher projectionDispatcher)
        {
            Contract.Requires(projectionDispatcher != null);

            _projectionDispatcher = projectionDispatcher;
        }

        public void Wait(TransactionIdentity transactionIdentity)
        {
            Wait(transactionIdentity, TimeSpan.MaxValue);
        }

        public void Wait(ICommandHandlingResult result)
        {
            Wait(result, TimeSpan.MaxValue);
        }
        
        public Task WaitAsync(TransactionIdentity transactionIdentity)
        {
            return WaitAsync(transactionIdentity, TimeSpan.MaxValue);
        }

        public Task WaitAsync(ICommandHandlingResult result)
        {            
            return WaitAsync(result, TimeSpan.MaxValue);
        }

        public void Wait(ICommandHandlingResult result, TimeSpan timeout)
        {
            var eventStoreCommandHandlingResult = result as EventStoreCommandHandlingResult;
            Contract.Assume(eventStoreCommandHandlingResult?.TransactionIdentity != null);

            Wait(eventStoreCommandHandlingResult.TransactionIdentity, timeout);
        }

        public Task WaitAsync(ICommandHandlingResult result, TimeSpan timeout)
        {
            var eventStoreCommandHandlingResult = result as EventStoreCommandHandlingResult;
            Contract.Assume(eventStoreCommandHandlingResult?.TransactionIdentity != null);

            return WaitAsync(eventStoreCommandHandlingResult.TransactionIdentity, timeout);
        }

        public void Wait(TransactionIdentity transactionIdentity, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (_projectionDispatcher.Processed(transactionIdentity) == false)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException(ProjectionTimeoutMessage);
                }

                Thread.Sleep(10);
            }
        }

        public async Task WaitAsync(TransactionIdentity transactionIdentity, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (_projectionDispatcher.Processed(transactionIdentity) == false)
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
            Contract.Invariant(_projectionDispatcher != null);
        }
    }
}
