using System;
using System.Runtime.Caching;

namespace DotNetRevolution.Core.Caching
{
    public class LazyCache : ICache
    {
        protected virtual string Region
        {
            get
            {
                return string.Empty;
            }
        }

        public T AddOrGetExisting<T>(string key, Lazy<T> lazy)
        {
            var newValue = lazy;            
            var oldValue = MemoryCache.Default.AddOrGetExisting(CreateKey(key), newValue, GetCacheItemPolicy()) as Lazy<T>;

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
            MemoryCache.Default.Remove(CreateKey(key));
        }
        
        protected virtual CacheItemPolicy GetCacheItemPolicy()
        {
            return new CacheItemPolicy();
        }

        protected virtual string CreateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(Region))
            {
                return key;
            }

            return $"{Region}::{key}";
        }
    }
}
