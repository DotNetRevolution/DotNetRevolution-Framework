﻿using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Messaging
{
    [Serializable]
    public class MessageHandlingException : Exception
    {
        public object MessageInstance { get; private set; }

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

        public MessageHandlingException(object value)
            : this(value, null, "Message was not handled correctly.")
        {
            Contract.Requires(value != null);
        }

        public MessageHandlingException(object value, string message)
            : this(value, null, message)
        {
            Contract.Requires(value != null);
        }

        public MessageHandlingException(object value, Exception innerException, string message)
            : base(message, innerException)
        {
            Contract.Requires(value != null);

            MessageInstance = value;
        }

        protected MessageHandlingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            MessageInstance = info.GetValue("MessageInstance", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MessageInstance", MessageInstance);

            base.GetObjectData(info, context);
        }
    }
}
