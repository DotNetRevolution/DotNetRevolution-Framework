using DotNetRevolution.Core.Caching;
using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace DotNetRevolution.Core.Domain
{
    public class AggregateRootSynchronizer : IAggregateRootSynchronizer
    {
        private readonly ICache _cache;

        public AggregateRootSynchronizer(ICache cache)
        {
            Contract.Requires(cache != null);

            _cache = cache;
        }

        public void Exit(Identity identity)
        {
            // exit lock for identity
            Monitor.Exit(identity);
        }

        public Identity Enter(Type aggregateRootType, Guid id)
        {
            Identity identity = null;

            try
            {
                var key = $"{aggregateRootType.FullName}::{id}";
                Contract.Assume(string.IsNullOrWhiteSpace(key) == false);

                // get and/or add identity to cache
                return identity = _cache.AddOrGetExisting(key, new Lazy<Identity>(() => new Identity(id)));
            }
            finally
            {
                // check if identity was returned from cache
                if (identity == null)
                {
                    throw new ApplicationException("Error creating or finding identity.");
                }

                // lock identity
                Monitor.Enter(identity);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_cache != null);
        }
    }
}
