using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class ReflectionCache : LazyCache
    {
        protected override string Region
        {
            get
            {
                return "Reflection";
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
