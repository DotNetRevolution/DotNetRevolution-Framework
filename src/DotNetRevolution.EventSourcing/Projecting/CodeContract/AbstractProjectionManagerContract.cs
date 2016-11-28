using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(ProjectionManager))]
    internal abstract class AbstractProjectionManagerContract : ProjectionManager
    {
        public AbstractProjectionManagerContract(IProjectionFactory projectionFactory) 
            : base(projectionFactory)
        {
            Contract.Requires(projectionFactory != null);
        }

        protected override void FinalizeProjection(IProjection projection, EventProviderTransaction transaction)
        {
            Contract.Requires(projection != null);
            Contract.Requires(transaction != null);

            throw new NotImplementedException();
        }

        protected override void FinalizeProjection(IProjection projection, Exception exception)
        {
            Contract.Requires(projection != null);
            Contract.Requires(exception != null);

            throw new NotImplementedException();
        }

        protected override void PrepareProjection(IProjection projection)
        {
            Contract.Requires(projection != null);
            
            throw new NotImplementedException();
        }

        protected override bool Processed(TransactionIdentity identity)
        {
            Contract.Requires(identity != null);

            throw new NotImplementedException();
        }

        protected override void SaveProjection(IProjection projection)
        {
            Contract.Requires(projection != null);

            throw new NotImplementedException();
        }
    }
}
