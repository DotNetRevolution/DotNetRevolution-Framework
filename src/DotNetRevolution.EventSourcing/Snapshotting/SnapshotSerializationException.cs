using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class SnapshotSerializationException : Exception
    {
        public SnapshotSerializationException()
        {
        }

        public SnapshotSerializationException(string message) : base(message)
        {
        }

        public SnapshotSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SnapshotSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
