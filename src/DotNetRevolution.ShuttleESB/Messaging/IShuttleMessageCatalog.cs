using DotNetRevolution.ShuttleESB.Messaging.CodeContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    [ContractClass(typeof(ShuttleMessageCatalogContract))]
    public interface IShuttleMessageCatalog
    {
        IReadOnlyCollection<IShuttleMessageEntry> Entries { get; }

        IShuttleMessageEntry this[Type messageType] { get; }

        void Add(IShuttleMessageEntry entry);
    }
}
