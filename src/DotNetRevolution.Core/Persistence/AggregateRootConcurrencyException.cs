using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Persistence
{
    public class AggregateRootConcurrencyException : Exception
    {
        public AggregateRootConcurrencyException()
        {
        }

        public AggregateRootConcurrencyException(string message) : base(message)
        {
        }

        public AggregateRootConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AggregateRootConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
