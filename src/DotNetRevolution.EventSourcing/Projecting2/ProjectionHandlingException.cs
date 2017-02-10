using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    [Serializable]
    internal class ProjectionHandlingException : Exception
    {
        public ProjectionHandlingException()
        {
        }

        public ProjectionHandlingException(string message) : base(message)
        {
        }

        public ProjectionHandlingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProjectionHandlingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}