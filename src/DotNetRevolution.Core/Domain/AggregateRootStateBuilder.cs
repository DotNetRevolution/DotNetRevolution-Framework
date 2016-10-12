using DotNetRevolution.Core.Caching;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Domain
{
    public class AggregateRootStateBuilder<TAggregateRootState> : IAggregateRootStateBuilder<TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
    {
        private static BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private readonly ReflectionCache _cache;

        public AggregateRootStateBuilder()
        {
            _cache = new ReflectionCache();
        }

        public TAggregateRootState Build(object snapshot)
        {
            ConstructorInfo ctor;
            TAggregateRootState state;

            // find constructor
            ctor = GetConstructor(new[] { snapshot.GetType() }, "SnapshotOnly");

            // invoke constructor creating state object
            state = ctor.Invoke(new[] { snapshot }) as TAggregateRootState;
            Contract.Assume(state != null);

            return state;
        }

        public TAggregateRootState Build(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            ConstructorInfo ctor;
            TAggregateRootState state;

            // find constructor
            ctor = GetConstructor(new[] { typeof(IReadOnlyCollection<IDomainEvent>) }, "DomainEventsOnly");

            // invoke constructor creating state object
            state = ctor.Invoke(new[] { domainEvents }) as TAggregateRootState;
            Contract.Assume(state != null);

            return state;
        }

        public TAggregateRootState Build(object snapshot, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            ConstructorInfo ctor;
            TAggregateRootState state;

            // find constructor
            ctor = GetConstructor(new[] { snapshot.GetType(), typeof(IReadOnlyCollection<IDomainEvent>) }, "SnapshotAndDomainEvents");

            // invoke constructor creating state object
            state = ctor.Invoke(new[] { snapshot, domainEvents }) as TAggregateRootState;
            Contract.Assume(state != null);

            return state;
        }

        private ConstructorInfo GetConstructor(Type[] parameters, string ctorType)
        {
            Contract.Requires(parameters != null);
            Contract.Ensures(Contract.Result<ConstructorInfo>() != null);

            var aggregateRootStateType = typeof(TAggregateRootState);

            var key = $"{aggregateRootStateType.FullName}::{ctorType}";
            Contract.Assume(string.IsNullOrWhiteSpace(key) == false);

            var ctor = _cache.AddOrGetExisting(key, new Lazy<ConstructorInfo>(() => aggregateRootStateType.GetConstructor(DefaultBindingFlags, Type.DefaultBinder, parameters, null)));
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
