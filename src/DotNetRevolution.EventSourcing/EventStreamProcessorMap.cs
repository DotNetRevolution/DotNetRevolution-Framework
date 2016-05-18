using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamProcessorMap
    {
        public Type DomainEventType { get; }

        public string MethodName { get; }

        public EventStreamProcessorMap(Type domainEventType, string methodName)
        {
            Contract.Requires(domainEventType != null);
            Contract.Requires(string.IsNullOrWhiteSpace(methodName) != false);

            DomainEventType = domainEventType;
            MethodName = methodName;
        }
    }
}
