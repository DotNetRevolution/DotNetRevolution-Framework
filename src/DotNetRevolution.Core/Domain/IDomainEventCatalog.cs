using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventCatalogContract))]
    public interface IDomainEventCatalog
    {        
        IReadOnlyCollection<Type> DomainEventTypes { [Pure] get; }
        
        IDomainEventEntryRegistration Add(IDomainEventEntry entry);

        [Pure]
        IReadOnlyCollection<IDomainEventEntry> GetEntries(Type domainEventType);

        void Remove(IDomainEventEntry entry);

        bool TryGetEntries(Type domainEventType, out IReadOnlyCollection<IDomainEventEntry> entries);
    }
}
 