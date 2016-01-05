using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventCatalogContract))]
    public interface IDomainEventCatalog
    {
        [Pure]
        IReadOnlyCollection<Type> DomainEventTypes { get; }

        [Pure]
        IReadOnlyCollection<IDomainEventEntry> this[Type domainEventType] { get; }

        IDomainEventEntryRegistration Add(IDomainEventEntry entry);

        void Remove(IDomainEventEntry entry);

        bool TryGetEntries(Type domainEventType, out IReadOnlyCollection<IDomainEventEntry> entries);
    }
}
