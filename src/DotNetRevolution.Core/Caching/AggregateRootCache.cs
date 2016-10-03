using System;
using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class AggregateRootCache : LazyCache
    {
        protected override string Region
        {
            get
            {
                return "AggregateRootCache";
            }
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
