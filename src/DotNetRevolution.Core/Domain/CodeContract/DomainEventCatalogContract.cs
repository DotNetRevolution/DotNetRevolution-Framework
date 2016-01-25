using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventCatalog))]
    public abstract class DomainEventCatalogContract : IDomainEventCatalog
    {
        public IReadOnlyCollection<Type> DomainEventTypes
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<Type>>() != null);

                throw new NotImplementedException();
            }
        }

        public IReadOnlyCollection<IDomainEventEntry> this[Type domainEventType]
        {
            get
            {
                Contract.Requires(domainEventType != null);
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEventEntry>>() != null);

                throw new NotImplementedException();
            }
        }

        public IDomainEventEntryRegistration Add(IDomainEventEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(this[entry.DomainEventType] != null);
            Contract.Ensures(Contract.Result<IDomainEventEntryRegistration>() != null);
            
            throw new NotImplementedException();
        }

        public void Remove(IDomainEventEntry entry)
        {
            Contract.Requires(entry != null);

            throw new NotImplementedException();
        }

        public bool TryGetEntries(Type domainEventType, out IReadOnlyCollection<IDomainEventEntry> entries)
        {
            Contract.Requires(domainEventType != null);

            throw new NotImplementedException();
        }
    }
}
