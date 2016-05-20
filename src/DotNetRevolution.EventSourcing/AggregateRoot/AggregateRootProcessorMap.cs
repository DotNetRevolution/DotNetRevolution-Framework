using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    public class AggregateRootProcessorMap
    {
        public Type DomainEventType { get; }

        public string MethodName { get; }

        public AggregateRootProcessorMap(Type domainEventType, string methodName)
        {
            Contract.Requires(domainEventType != null);
            Contract.Requires(string.IsNullOrWhiteSpace(methodName) == false);

            DomainEventType = domainEventType;
            MethodName = methodName;
        }
    }
}
