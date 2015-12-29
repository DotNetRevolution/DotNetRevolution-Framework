using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Messaging
{
    [Serializable]
    public class MessageHandlingException : Exception
    {
        public object MessageObject { get; private set; }

        public MessageHandlingException()
        {
        }

        public MessageHandlingException(string message)
            : base(message)
        {
        }

        public MessageHandlingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MessageHandlingException(object messageObject)
            : this(messageObject, null, "Message was not handled correctly.")
        {
            Contract.Requires(messageObject != null);
        }

        public MessageHandlingException(object messageObject, string message)
            : this(messageObject, null, message)
        {
            Contract.Requires(messageObject != null);
        }

        public MessageHandlingException(object messageObject, Exception innerException, string message)
            : base(message, innerException)
        {
            Contract.Requires(messageObject != null);

            MessageObject = messageObject;
        }

        protected MessageHandlingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            MessageObject = info.GetValue("MessageObject", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MessageObject", MessageObject);

            base.GetObjectData(info, context);
        }
    }
}