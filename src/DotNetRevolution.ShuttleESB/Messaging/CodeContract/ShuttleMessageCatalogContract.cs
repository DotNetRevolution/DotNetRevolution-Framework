using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.ShuttleESB.Messaging.CodeContract
{
    [ContractClassFor(typeof(IShuttleMessageCatalog))]
    public abstract class ShuttleMessageCatalogContract : IShuttleMessageCatalog
    {
        public IShuttleMessageEntry this[Type messageType]
        {
            get
            {
                Contract.Requires(messageType != null);
                Contract.Ensures(Contract.Result<IShuttleMessageEntry>() != null);

                throw new NotImplementedException();
            }
        }

        public IReadOnlyCollection<IShuttleMessageEntry> Entries
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IShuttleMessageEntry>>() != null);

                throw new NotImplementedException();
            }
        }

        public void Add(IShuttleMessageEntry entry)
        {
            Contract.Requires(entry != null);
        }
    }
}
