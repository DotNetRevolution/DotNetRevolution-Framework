using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    public class MemoryProjectionFactory : IProjectionFactory
    {
        private readonly Projection _projection;

        public MemoryProjectionFactory(Projection projection)
        {
            Contract.Requires(projection != null);

            _projection = projection;
        }

        public IProjection GetProjection()
        {
            return _projection;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projection != null);
        }
    }
}
