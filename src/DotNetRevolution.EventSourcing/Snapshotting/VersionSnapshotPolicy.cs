using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class VersionSnapshotPolicy : ISnapshotPolicy
    {
        private readonly int _modulus;

        public VersionSnapshotPolicy(int modulus)
        {
            Contract.Requires(modulus > 0);

            _modulus = modulus;
        }
        
        public bool Check(IEventStream eventStream)
        {
            return true;//(eventStream.Version % _modulus) == 0;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_modulus > 0);
        }
    }
}
