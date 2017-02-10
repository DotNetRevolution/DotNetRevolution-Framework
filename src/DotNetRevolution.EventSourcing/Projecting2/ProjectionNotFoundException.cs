using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    [Serializable]
    public class ProjectionNotFoundException : Exception
    {
        public ProjectionNotFoundException()
        {
        }

        public ProjectionNotFoundException(string message) : base(message)
        {
        }

        public ProjectionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProjectionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}