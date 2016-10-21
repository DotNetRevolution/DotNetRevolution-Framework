using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Projecting
{
    public abstract class ProjectionManager<TProjection, TAggregateRoot>
        where TProjection : Projection<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        public abstract bool Processed(TransactionIdentity transactionIdentity);

        public void Wait(TransactionIdentity transactionIdentity)
        {
            Contract.Requires(transactionIdentity != null);

            Wait(transactionIdentity, TimeSpan.MaxValue);
        }

        public void Wait(TransactionIdentity transactionIdentity, TimeSpan timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            while (Processed(transactionIdentity) == false)
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

            while (Processed(transactionIdentity) == false)
            {
                if (stopwatch.Elapsed > timeout)
                {
                    throw new TimeoutException("Projection did not process transaction before the timeout period elapsed");
                }

                await Task.Delay(10);
            }
        }
    }
}
