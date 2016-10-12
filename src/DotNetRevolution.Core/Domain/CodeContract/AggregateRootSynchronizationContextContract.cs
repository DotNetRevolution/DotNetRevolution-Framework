using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootSynchronizationContext))]
    internal abstract class AggregateRootSynchronizationContextContract : IAggregateRootSynchronizationContext
    {
        public Identity Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<Identity>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract void Dispose();

        public void Lock()
        {
            throw new NotImplementedException();
        }

        public Task LockAsync()
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public void Unlock()
        {
            throw new NotImplementedException();
        }
    }
}
