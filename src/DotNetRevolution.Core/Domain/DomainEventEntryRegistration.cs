using DotNetRevolution.Core.Base;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventEntryRegistration : Disposable, IDomainEventEntryRegistration
    {
        private readonly IDomainEventCatalog _catalog;        

        public IDomainEventEntry Entry { get; }

        internal DomainEventEntryRegistration(IDomainEventCatalog catalog, IDomainEventEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Requires(catalog != null);
                    
            _catalog = catalog;
            Entry = entry;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _catalog.Remove(Entry);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Entry != null);
            Contract.Invariant(_catalog != null);
        }
    }
}
