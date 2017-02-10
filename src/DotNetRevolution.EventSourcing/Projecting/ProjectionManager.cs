using DotNetRevolution.Core.Extension;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(AbstractProjectionManagerContract))]
    public abstract class ProjectionManager : IProjectionManager
    {
        private readonly Collection<TransactionIdentity> _processedTransactions = new Collection<TransactionIdentity>();
        private readonly IProjectionFactory _projectionFactory;

        public IReadOnlyCollection<TransactionIdentity> ProcessedTransactions
        {
            get
            {
                return _processedTransactions;
            }
        }

        protected ProjectionManager(IProjectionFactory projectionFactory)
        {
            Contract.Requires(projectionFactory != null);

            _projectionFactory = projectionFactory;
        }
        
        public void Handle(IEventProvider eventProvider, params IProjectionContext[] projectionContexts)
        {
            // get projection
            var projection = _projectionFactory.GetProjection(eventProvider);

            // project
            projectionContexts.ForEach(projection.Project);

            // save transaction identity to know its been processed
            projectionContexts.Select(x => x.TransactionIdentity)
                .Distinct()
                .ForEach(_processedTransactions.Add);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_processedTransactions != null);
            Contract.Invariant(_projectionFactory != null);
        }
    }
}
