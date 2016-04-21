using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Domain
{
    public class MissingApplyDomainEventMappingException : Exception
    {
        public Type AggregateType { get; }

        public Type DomainEventType { get; }

        public MissingApplyDomainEventMappingException(Type aggregateType, Type domainEventType)
        {
            Contract.Requires(aggregateType != null);
            Contract.Requires(domainEventType != null);

            AggregateType = aggregateType;
            DomainEventType = domainEventType;
        }

        public MissingApplyDomainEventMappingException()
        {
        }

        public MissingApplyDomainEventMappingException(string message) 
            : base(message)
        {
        }

        public MissingApplyDomainEventMappingException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected MissingApplyDomainEventMappingException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
