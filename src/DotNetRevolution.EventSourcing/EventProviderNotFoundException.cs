using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderNotFoundException : Exception
    {
        public EventProviderNotFoundException()
        {
        }

        public EventProviderNotFoundException(string message) 
            : base(message)
        {
        }

        public EventProviderNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected EventProviderNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
