using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.EventSourcing
{
    public class DomainEventSerializationException : Exception
    {
        public DomainEventSerializationException()
        {
        }

        public DomainEventSerializationException(string message) : base(message)
        {
        }

        public DomainEventSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DomainEventSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
