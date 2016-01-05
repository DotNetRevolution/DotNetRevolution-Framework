using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventEntryRegistration : Disposable, IDomainEventEntryRegistration
    {
        private readonly IDomainEventCatalog _catalog;
        private readonly IDomainEventEntry _entry;

        public Guid Id { get; private set; }

        internal DomainEventEntryRegistration(IDomainEventCatalog catalog, IDomainEventEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Requires(catalog != null);

            Id = Guid.NewGuid();

            _catalog = catalog;
            _entry = entry;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _catalog.Remove(_entry);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_entry != null);
            Contract.Invariant(_catalog != null);
        }
    }
}
