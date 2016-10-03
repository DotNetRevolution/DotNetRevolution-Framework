using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class ReflectionCache : RegionedLazyCache
    {        
        public ReflectionCache()
            : base("Reflection")
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
