using System;
using Shuttle.Esb;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.ShuttleESB.Messaging.CodeContract
{
    [ContractClassFor(typeof(IShuttleMessageEntry))]
    internal abstract class ShuttleMessageEntryContract : IShuttleMessageEntry
    {
        public IMessageHandler MessageHandler
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Type MessageHandlerType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public Type MessageType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
