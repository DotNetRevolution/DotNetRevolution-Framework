using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessage))]
    internal abstract class MessageContract : IMessage
    {
        public Guid MessageId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);

                throw new NotImplementedException();
            }
        }
    }
}
