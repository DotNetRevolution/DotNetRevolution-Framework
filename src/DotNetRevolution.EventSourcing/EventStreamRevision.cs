using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStreamRevision
    {
        private readonly EventProviderVersion _version;
        private readonly EventStreamRevisionIdentity _identity;

        [Pure]
        public EventStreamRevisionIdentity Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<EventStreamRevisionIdentity>() != null);

                return _identity;
            }
        }

        [Pure]
        public EventProviderVersion Version
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

                return _version;
            }
        }

        protected EventStreamRevision(EventStreamRevisionIdentity identity, EventProviderVersion version)
        {
            Contract.Requires(identity != null);
            Contract.Requires(version != null);

            _identity = identity;
            _version = version;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identity != null);
            Contract.Invariant(_version != null);
        }
    }
}
