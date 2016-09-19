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

        protected static class Cache<TCache>
        {
            public static IDictionary<Type[], ConstructorInfo> Entries = new Dictionary<Type[], ConstructorInfo>();

            public static object Lock = new object();
        }

        public TAggregateRootState Build(object snapshot)
        {
            ConstructorInfo ctor;
            TAggregateRootState state;

            // find constructor
            ctor = GetConstructor(new[] { snapshot.GetType() });
            Contract.Assume(ctor != null);

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
            ctor = GetConstructor(new[] { typeof(IReadOnlyCollection<IDomainEvent>) });
            Contract.Assume(ctor != null);

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
            ctor = GetConstructor(new[] { snapshot.GetType(), typeof(IReadOnlyCollection<IDomainEvent>) });
            Contract.Assume(ctor != null);

            // invoke constructor creating state object
            state = ctor.Invoke(new[] { snapshot, domainEvents }) as TAggregateRootState;
            Contract.Assume(state != null);

            return state;
        }

        private static ConstructorInfo GetConstructor(Type[] parameters)
        {
            Contract.Requires(parameters != null);
            Contract.Ensures(Contract.Result<ConstructorInfo>() != null);

            ConstructorInfo ctor;

            if (Cache<TAggregateRootState>.Entries.TryGetValue(parameters, out ctor))
            {
                Contract.Assume(ctor != null);
            }
            else
            {
                lock (Cache<TAggregateRootState>.Lock)
                {
                    if (Cache<TAggregateRootState>.Entries.TryGetValue(parameters, out ctor))
                    {
                        Contract.Assume(ctor != null);
                    }
                    else
                    {
                        ctor = typeof(TAggregateRootState).GetConstructor(DefaultBindingFlags, Type.DefaultBinder, parameters, null);
                        Contract.Assume(ctor != null);

                        Cache<TAggregateRootState>.Entries.Add(parameters, ctor);
                    }
                }
            }

            return ctor;
        }
    }
}
