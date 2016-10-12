using DotNetRevolution.Core.Caching;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain
{
    public class AggregateRootSynchronizer : IAggregateRootSynchronizer
    {
        private const string ErrorMessage = "Error creating or finding synchronization context.";
        private readonly ICache _cache;        

        public AggregateRootSynchronizer(ICache cache)
        {
            Contract.Requires(cache != null);

            _cache = cache;
        }

        public void Exit(IAggregateRootSynchronizationContext context)
        {
            // exit lock for identity
            context.Unlock();
        }

        public IAggregateRootSynchronizationContext Enter(Type aggregateRootType, Guid aggregateRootId)
        {
            IAggregateRootSynchronizationContext context = null;

            try
            {
                string key = GetKey(aggregateRootType, aggregateRootId);

                // get and/or add context to cache
                return context = AddOrGetCacheItem(aggregateRootId, key);
            }
            finally
            {
                // check if context was returned from cache
                if (context == null)
                {
                    throw new ApplicationException(ErrorMessage);
                }

                // lock identity
                context.Lock();
            }
        }

        public async Task<IAggregateRootSynchronizationContext> EnterAsync(Type aggregateRootType, Guid aggregateRootId)
        {
            IAggregateRootSynchronizationContext context = null;

            try
            {
                string key = GetKey(aggregateRootType, aggregateRootId);

                // get and/or add context to cache
                return context = AddOrGetCacheItem(aggregateRootId, key);
            }
            finally
            {
                // check if context was returned from cache
                if (context == null)
                {
                    throw new ApplicationException(ErrorMessage);
                }

                // lock identity
                await context.LockAsync();
            }
        }

        protected virtual AggregateRootSynchronizationContext CreateAggregateRootSynchronizationContext(Guid aggregateRootId)
        {
            Contract.Requires(aggregateRootId != Guid.Empty);

            return new AggregateRootSynchronizationContext(new Identity(aggregateRootId));
        }

        private IAggregateRootSynchronizationContext AddOrGetCacheItem(Guid aggregateRootId, string key)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key));

            return _cache.AddOrGetExisting(key, new Lazy<IAggregateRootSynchronizationContext>(() => CreateAggregateRootSynchronizationContext(aggregateRootId)));
        }

        private static string GetKey(Type aggregateRootType, Guid aggregateRootId)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(string.IsNullOrWhiteSpace(Contract.Result<string>()) == false);

            var key = $"{aggregateRootType.FullName}::{aggregateRootId}";
            Contract.Assume(string.IsNullOrWhiteSpace(key) == false);

            return key;
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_cache != null);
        }
    }
}
