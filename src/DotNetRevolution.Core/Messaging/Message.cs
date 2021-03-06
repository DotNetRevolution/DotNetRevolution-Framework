﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public abstract class Message : IMessage
    {
        public Guid MessageId { get; }

        protected Message(Guid messageId)
        {
            Contract.Requires(messageId != Guid.Empty);

            MessageId = messageId;
        }
    }
}
