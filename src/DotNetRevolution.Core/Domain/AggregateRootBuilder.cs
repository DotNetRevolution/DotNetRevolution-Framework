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

        public TAggregateRoot Build(Identity identity, TAggregateRootState state)
        {
            ConstructorInfo ctor = typeof(TAggregateRoot).GetConstructor(DefaultBindingFlags, Type.DefaultBinder, new[] { typeof(Identity), typeof(TAggregateRootState) }, null);
            Contract.Assume(ctor != null);

            TAggregateRoot aggregateRoot = ctor.Invoke(new object[] { identity, state }) as TAggregateRoot;
            Contract.Assume(aggregateRoot != null);

            return aggregateRoot;           
        }
    }
}
