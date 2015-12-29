using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Domain
{
    [Serializable]
    public class DomainEventHandlingException : Exception
    {
        public object DomainEvent { get; private set; }

        public DomainEventHandlingException()
        {
        }

        public DomainEventHandlingException(string message)
            : base(message)
        {
        }

        public DomainEventHandlingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DomainEventHandlingException(object domainEvent)
            : this(domainEvent, null, "Domain event was not handled correctly.")
        {
            Contract.Requires(domainEvent != null);
        }

        public DomainEventHandlingException(object domainEvent, string message)
            : this(domainEvent, null, message)
        {
            Contract.Requires(domainEvent != null);
        }

        public DomainEventHandlingException(object domainEvent, Exception innerException, string message)
            : base(message, innerException)
        {
            Contract.Requires(domainEvent != null);

            DomainEvent = domainEvent;
        }

        protected DomainEventHandlingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            DomainEvent = info.GetValue("DomainEvent", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DomainEvent", DomainEvent);

            base.GetObjectData(info, context);
        }
    }
}