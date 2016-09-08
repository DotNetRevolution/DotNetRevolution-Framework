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

        public TAggregateRootState Build(object snapshot)
        {            
            ConstructorInfo ctor;
            TAggregateRootState state;

            // find constructor
            ctor = typeof(TAggregateRootState).GetConstructor(DefaultBindingFlags, Type.DefaultBinder, new[] { snapshot.GetType() }, null);
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
            ctor = typeof(TAggregateRootState).GetConstructor(DefaultBindingFlags, Type.DefaultBinder, new[] { typeof(IReadOnlyCollection<IDomainEvent>) }, null);
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
            ctor = typeof(TAggregateRootState).GetConstructor(DefaultBindingFlags, Type.DefaultBinder, new[] { snapshot.GetType(), typeof(IReadOnlyCollection<IDomainEvent>) }, null);
            Contract.Assume(ctor != null);

            // invoke constructor creating state object
            state = ctor.Invoke(new[] { snapshot, domainEvents }) as TAggregateRootState;
            Contract.Assume(state != null);
            
            return state;
        }
    }
}
