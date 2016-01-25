﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageCatalog))]
    public abstract class MessageCatalogContract : IMessageCatalog
    {
        public IMessageEntry this[Type messageType]
        {
            get
            {
                Contract.Requires(messageType != null);

                throw new NotImplementedException();
            }
        }

        public void Add(IMessageEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(this[entry.MessageType] != null);
        }
    }
}
