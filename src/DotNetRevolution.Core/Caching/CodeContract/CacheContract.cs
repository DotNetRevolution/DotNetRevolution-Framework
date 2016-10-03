using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Caching.CodeContract
{
    [ContractClassFor(typeof(ICache))]
    internal abstract class CacheContract : ICache
    {
        public T AddOrGetExisting<T>(string key, Lazy<T> lazy)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(key) == false);
            Contract.Requires(lazy != null);

            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(key) == false);

            throw new NotImplementedException();
        }
    }
}
