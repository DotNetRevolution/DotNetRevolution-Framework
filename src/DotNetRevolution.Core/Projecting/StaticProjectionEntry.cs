using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    public class StaticProjectionEntry : ProjectionEntry
    {
        private readonly IProjectionManager _manager;

        public IProjectionManager Manager
        {
            get
            {
                Contract.Ensures(Contract.Result<IProjectionManager>() != null);

                return _manager;
            }
        }

        public StaticProjectionEntry(Type projectionType, IProjectionManager manager)
            : base(projectionType, manager.GetType())
        {
            Contract.Requires(projectionType != null);
            Contract.Requires(manager != null);

            _manager = manager;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_manager != null);
        }
    }
}
