using DotNetRevolution.Core.Base;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain
{
    public class AggregateRootSynchronizationContext : Disposable, IAggregateRootSynchronizationContext
    {
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly Identity _identity;

        public Identity Identity
        {
            get
            {
                return _identity;
            }
        }

        public AggregateRootSynchronizationContext(Identity identity)
        {
            Contract.Requires(identity != null);

            _identity = identity;
        }

        public void Lock()
        {
            _lock.Wait();
        }

        public Task LockAsync()
        {
            var task = _lock.WaitAsync();
            Contract.Assume(task != null);

            return task;
        }

        public void Unlock()
        {
            _lock.Release();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unlock();
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identity != null);
            Contract.Invariant(_lock != null);
        }
    }
}
