using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStreamProcessor : IEventStreamProcessor
    {
        protected BindingFlags DefaultBindingFlags
        {
            [Pure]
            get
            {
                return BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            }
        }

        public TAggregateRoot Process<TAggregateRoot>(EventStream eventStream) where TAggregateRoot : class
        {
            var aggregateRootType = typeof(TAggregateRoot);

            var aggregateRoot = Activator.CreateInstance(aggregateRootType, true) as TAggregateRoot;

            if (aggregateRoot == null)
            {
                throw new InvalidOperationException(string.Format("Could not create an instance of {0}", aggregateRootType.FullName));
            }

            foreach (var domainEvent in eventStream)
            {
                Contract.Assume(domainEvent != null);

                var methodName = GetMethodName(domainEvent.GetType().Name);

                var methodInfo = aggregateRootType.GetMethod(methodName, DefaultBindingFlags, null, new[] { domainEvent.GetType() }, null);

                if (methodInfo == null)
                {
                    throw new InvalidOperationException(string.Format("Could not find method {0}({1})", methodName, domainEvent.GetType().FullName));
                }

                methodInfo.Invoke(aggregateRoot, new[] { domainEvent });
            }

            return aggregateRoot;
        }

        protected abstract string GetMethodName(string domainEventName);
    }
}
