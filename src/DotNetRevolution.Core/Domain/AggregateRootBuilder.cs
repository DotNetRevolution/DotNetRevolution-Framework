using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Domain
{
    public class AggregateRootBuilder<TAggregateRoot, TAggregateRootState> : IAggregateRootBuilder<TAggregateRoot, TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
    {
        protected static class Cache<TCache>
        {
            public static ConstructorInfo Constructor { get; set; }

            public static object Lock = new object();
        }

        private static BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public TAggregateRoot Build(Identity identity, TAggregateRootState state)
        {
            ConstructorInfo ctor = GetConstructor();
            Contract.Assume(ctor != null);

            TAggregateRoot aggregateRoot = ctor.Invoke(new object[] { identity, state }) as TAggregateRoot;
            Contract.Assume(aggregateRoot != null);

            return aggregateRoot;
        }

        private static ConstructorInfo GetConstructor()
        {
            var ctor = Cache<TAggregateRoot>.Constructor;

            if (ctor == null)
            {
                lock(Cache<TAggregateRoot>.Lock)
                {
                    if (Cache<TAggregateRoot>.Constructor == null)
                    {
                        ctor = typeof(TAggregateRoot).GetConstructor(DefaultBindingFlags, Type.DefaultBinder, new[] { typeof(Identity), typeof(TAggregateRootState) }, null);
                        Contract.Assume(ctor != null);

                        Cache<TAggregateRoot>.Constructor = ctor;
                    }
                    else
                    {
                        ctor = Cache<TAggregateRoot>.Constructor;
                    }
                }
            }

            return ctor;
        }
    }
}
