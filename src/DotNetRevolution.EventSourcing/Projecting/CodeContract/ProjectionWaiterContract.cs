using DotNetRevolution.Core.Commanding;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionWaiter))]
    internal abstract class ProjectionWaiterContract : IProjectionWaiter
    {
        public void Wait(TransactionIdentity transactionIdentity)
        {
            Contract.Requires(transactionIdentity != null);

            throw new NotImplementedException();
        }

        public void Wait(ICommandHandlingResult result)
        {
            Contract.Requires(result != null);

            throw new NotImplementedException();
        }

        public void Wait(TransactionIdentity transactionIdentity, TimeSpan timeout)
        {
            Contract.Requires(transactionIdentity != null);

            throw new NotImplementedException();
        }

        public void Wait(ICommandHandlingResult result, TimeSpan timeout)
        {
            Contract.Requires(result != null);

            throw new NotImplementedException();
        }

        public Task WaitAsync(TransactionIdentity transactionIdentity)
        {
            Contract.Requires(transactionIdentity != null);

            throw new NotImplementedException();
        }

        public Task WaitAsync(ICommandHandlingResult result)
        {
            Contract.Requires(result != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public Task WaitAsync(TransactionIdentity transactionIdentity, TimeSpan timeout)
        {
            Contract.Requires(transactionIdentity != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public Task WaitAsync(ICommandHandlingResult result, TimeSpan timeout)
        {
            Contract.Requires(result != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
