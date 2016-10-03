using DotNetRevolution.Core.Caching;
using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Domain
{
    public class AggregateRootBuilder<TAggregateRoot, TAggregateRootState> : IAggregateRootBuilder<TAggregateRoot, TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
    {
        private static BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private readonly ReflectionCache _cache;

        public AggregateRootBuilder()
        {
            _cache = new ReflectionCache();
        }

        public TAggregateRoot Build(Identity identity, TAggregateRootState state)
        {
            ConstructorInfo ctor = GetConstructor();

            TAggregateRoot aggregateRoot = ctor.Invoke(new object[] { identity, state }) as TAggregateRoot;
            Contract.Assume(aggregateRoot != null);

            return aggregateRoot;
        }

        private ConstructorInfo GetConstructor()
        {
            Contract.Ensures(Contract.Result<ConstructorInfo>() != null);

            var aggregateRootType = typeof(TAggregateRoot);

            var key = $"{aggregateRootType.FullName}";
            Contract.Assume(string.IsNullOrWhiteSpace(key) == false);

            var ctor = _cache.AddOrGetExisting(key, new Lazy<ConstructorInfo>(() => aggregateRootType.GetConstructor(DefaultBindingFlags, Type.DefaultBinder, new[] { typeof(Identity), typeof(TAggregateRootState) }, null)));
            Contract.Assume(ctor != null);

            return ctor;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_cache != null);
        }
    }
}
