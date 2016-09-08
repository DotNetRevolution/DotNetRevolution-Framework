using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStreamRevision : IEventStreamRevision
    {
        private readonly EventProviderVersion _version;
                
        [Pure]
        public EventProviderVersion Version
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

                return _version;
            }
        }
        
        [Pure]
        public bool Committed { get; private set; }
                
        public EventStreamRevision(EventProviderVersion version, bool committed)
        {
            Contract.Requires(version != null);
                        
            _version = version;
            Committed = committed;
        }
        
        internal void Commit()
        {
            Committed = true;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_version != null);
        }
    }
}
