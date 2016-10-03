using System;
using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class AggregateRootCache : RegionedLazyCache
    {
        public AggregateRootCache()
            : base("AggregateRootCache")
        {
        }

        protected override CacheItemPolicy GetCacheItemPolicy()
        {
            return new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
        }
    }
}
