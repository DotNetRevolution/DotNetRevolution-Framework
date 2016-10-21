using System;
using DotNetRevolution.Core.Messaging;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class Message2 : Message
    {
        public Message2(Guid messageId) 
            : base(messageId)
        {
            Contract.Requires(messageId != Guid.Empty);
        }
    }
}
