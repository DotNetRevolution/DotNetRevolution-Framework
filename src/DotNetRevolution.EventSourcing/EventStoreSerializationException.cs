using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.EventSourcing
{
    [Serializable]
    public class EventStoreSerializationException : Exception
    {
        public EventStoreSerializationException()
        {
        }

        public EventStoreSerializationException(string message) : base(message)
        {
        }

        public EventStoreSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EventStoreSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
