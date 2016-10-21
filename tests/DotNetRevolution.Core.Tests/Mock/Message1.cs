using System;
using DotNetRevolution.Core.Messaging;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class Message1 : Message
    {
        public Message1(Guid messageId) 
            : base(messageId)
        {
            Contract.Requires(messageId != Guid.Empty);
        }
    }
}
