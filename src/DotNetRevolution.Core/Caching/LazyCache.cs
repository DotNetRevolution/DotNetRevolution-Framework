using System;
using System.Diagnostics.Contracts;
using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class LazyCache : ICache
    {
        public T AddOrGetExisting<T>(string key, Lazy<T> lazy)
        {
            var newValue = lazy;            
            var oldValue = MemoryCache.Default.AddOrGetExisting(AddRegionToKey(key), newValue, GetCacheItemPolicy()) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                Remove(key);
                throw;
            }
        }
        
        public void Remove(string key)
        {
            MemoryCache.Default.Remove(AddRegionToKey(key));
        }
        
        protected virtual CacheItemPolicy GetCacheItemPolicy()
        {
            return new CacheItemPolicy();
        }

        protected virtual string AddRegionToKey(string key)
        {
            return key;
        }
    }
}
