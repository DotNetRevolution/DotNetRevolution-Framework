using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class AggregateRootSynchronizationCache : LazyCache
    {
        protected override string Region
        {
            get
            {
                return "AggregateRootSynchronizationCache";
            }
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
