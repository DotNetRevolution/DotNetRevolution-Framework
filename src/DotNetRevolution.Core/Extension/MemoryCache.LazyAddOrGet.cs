using System;
using System.Diagnostics.Contracts;
using System.Runtime.Caching;

namespace DotNetRevolution.Core.Extension
{
    public static class MemoryCacheExtension
    {
        public static T AddOrGetExisting<T>(this MemoryCache cache, string key, Func<T> valueFactory, CacheItemPolicy policy)
        {
            Contract.Requires(cache != null);
            Contract.Requires(valueFactory != null);

            var newValue = new Lazy<T>(valueFactory);
            var oldValue = cache.AddOrGetExisting(key, newValue, policy) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                cache.Remove(key);
                throw;
            }
        }

        public static T AddOrGetExisting<T>(this MemoryCache cache, string key, Func<T> valueFactory, CacheItemPolicy policy, string region)
        {
            Contract.Requires(cache != null);
            Contract.Requires(valueFactory != null);
            Contract.Requires(string.IsNullOrWhiteSpace(region) == false);

            return AddOrGetExisting(cache, $"{region}::{key}", valueFactory, policy);
        }
    }
}
