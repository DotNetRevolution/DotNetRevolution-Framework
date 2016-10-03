using DotNetRevolution.Core.Caching.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Caching
{
    [ContractClass(typeof(CacheContract))]
    public interface ICache
    {
        T AddOrGetExisting<T>(string key, Lazy<T> lazy);

        void Remove(string key);
    }
}