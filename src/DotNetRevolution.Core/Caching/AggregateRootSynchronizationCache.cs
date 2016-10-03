using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class AggregateRootSynchronizationCache : RegionedLazyCache
    {
        public AggregateRootSynchronizationCache()
            : base("AggregateRootSynchronizationCache")
        {
        }
        
        protected override CacheItemPolicy GetCacheItemPolicy()
        {
            return new CacheItemPolicy
            {
                Priority = CacheItemPriority.NotRemovable
            };
        }
    }
}
