using DotNetRevolution.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Projecting
{
    public class MemoryProjection : Projection
    {
        private readonly HashSet<TransactionIdentity> _processedTransactions = new HashSet<TransactionIdentity>();

        public MemoryProjection(ProjectionIdentity projectionIdentity)
            : base(projectionIdentity)
        {
            Contract.Requires(projectionIdentity != null);
        }

        public bool Processed(TransactionIdentity transactionIdentity)
        {
            return false;
        }
        
        public void Wait(TransactionIdentity transactionIdentity)
        {
            Contract.Requires(transactionIdentity != null);

            Wait(transactionIdentity, TimeSpan.MaxValue);
        }

        public void Wait(TransactionIdentity transactionIdentity, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (_processedTransactions.Contains(transactionIdentity) == false)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException("Projection did not process transaction before the timeout period elapsed");
                }

                Thread.Sleep(10);
            }
        }

        public Task WaitAsync(TransactionIdentity transactionIdentity)
        {
            Contract.Requires(transactionIdentity != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            return WaitAsync(transactionIdentity, TimeSpan.MaxValue);
        }

        public async Task WaitAsync(TransactionIdentity transactionIdentity, TimeSpan timeout)
        {
            Contract.Requires(transactionIdentity != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            var stopwatch = Stopwatch.StartNew();

            while (_processedTransactions.Contains(transactionIdentity) == false)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException("Projection did not process transaction before the timeout period elapsed");
                }

                await Task.Delay(10);
            }
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_processedTransactions != null);
        }
    }
}
