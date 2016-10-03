using DotNetRevolution.Core.Caching;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace DotNetRevolution.Core.Reflection
{
    public class StartsWithMethodInvoker<T> : MethodInvoker
    {
        private readonly ICache _cache;
        private readonly string _methodName;

        public StartsWithMethodInvoker(string methodName)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(methodName) == false);

            _methodName = methodName;
            _cache = new ReflectionCache();
        }

        protected override IDictionary<Type, MethodInfo> GetCachedEntries<TInstance>()
        {
            var type = typeof(TInstance);

            return _cache.AddOrGetExisting(type.FullName, new Lazy<IDictionary<Type, MethodInfo>>(() => type
                                                                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                                                    .Where(m => m.Name.StartsWith(_methodName))
                                                                    .Where(m => m.GetParameters().Length == 1)
                                                                    .ToDictionary(m => m.GetParameters().First().ParameterType, m => m)));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_cache != null);
        }
    }
}
