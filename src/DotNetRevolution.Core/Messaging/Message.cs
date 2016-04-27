using DotNetRevolution.Core.Base;
using System;

namespace DotNetRevolution.Core.Messaging
{
    public abstract class Message : IMessage
    {
        private readonly Guid _id = SequentialGuid.Create();

        public Guid Id
        {
            get
            {
                return _id;
            }
        }
    }
}
