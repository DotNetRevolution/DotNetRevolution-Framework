using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using DotNetRevolution.EventSourcing.Snapshotting;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    public abstract class AggregateRootProcessor : IAggregateRootProcessor
    {
        protected BindingFlags DefaultBindingFlags
        {
            [Pure]
            get
            {
                return BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            }
        }

        public TAggregateRoot Process<TAggregateRoot>(Snapshot snapshot) where TAggregateRoot : class, IAggregateRoot
        {
            return CreateAggregateRoot<TAggregateRoot>(typeof(TAggregateRoot), snapshot);
        }

        public TAggregateRoot Process<TAggregateRoot>(EventStream eventStream) where TAggregateRoot : class, IAggregateRoot
        {
            var aggregateRootType = typeof(TAggregateRoot);

            // create aggregate root
            TAggregateRoot aggregateRoot = CreateAggregateRoot<TAggregateRoot>(aggregateRootType, eventStream.Snapshot);

            // apply domain events to bring to current state
            foreach (var domainEvent in eventStream)
            {
                Contract.Assume(domainEvent != null);

                // get apply method name for domain event
                var methodName = GetMethodName(domainEvent.GetType().Name);

                // get apply method info
                var methodInfo = aggregateRootType.GetMethod(methodName, DefaultBindingFlags, null, new[] { domainEvent.GetType() }, null);

                // check if method info found
                if (methodInfo == null)
                {
                    throw new InvalidOperationException(string.Format("Could not find method {0}({1})", methodName, domainEvent.GetType().FullName));
                }

                // invoke method supplying domain event
                methodInfo.Invoke(aggregateRoot, new[] { domainEvent });
            }

            return aggregateRoot;
        }

        private TAggregateRoot CreateAggregateRoot<TAggregateRoot>(Type aggregateRootType, Snapshot snapshot) where TAggregateRoot : class, IAggregateRoot
        {            
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            TAggregateRoot aggregateRoot;

            // if snapshot is null, try using default ctor
            if (snapshot == null)
            {
                aggregateRoot = Activator.CreateInstance(aggregateRootType, true) as TAggregateRoot;
            }
            else
            {                
                // find ctor from snapshot type
                var ctor = aggregateRootType.GetConstructor(DefaultBindingFlags, Type.DefaultBinder, new[] { snapshot.Data.GetType() }, null);
                                
                // make sure ctor was found
                if (ctor == null)
                {
                    throw new InvalidOperationException(string.Format("Aggregate root does not have a constructor with a snapshot parameter of {0}", snapshot.Data.GetType().FullName));
                }

                // invoke ctor with snapshot
                aggregateRoot = ctor.Invoke(new[] { snapshot.Data }) as TAggregateRoot;
            }

            // need to make sure aggregate root was created
            if (aggregateRoot == null)
            {
                throw new InvalidOperationException(string.Format("Could not create an instance of {0}", aggregateRootType.FullName));
            }

            return aggregateRoot;
        }

        protected abstract string GetMethodName(string domainEventName);
    }
}
