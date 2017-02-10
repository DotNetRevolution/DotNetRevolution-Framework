using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionEntry))]
    internal abstract class ProjectionEntryContract : IProjectionEntry
    {
        public AggregateRootType AggregateRootType
        {
            get
            {
                Contract.Ensures(Contract.Result<AggregateRootType>() != null);

                throw new NotImplementedException();
            }
        }

        public IProjectionManager ProjectionManager
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Type ProjectionManagerType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public ProjectionType ProjectionType
        {
            get
            {
                Contract.Ensures(Contract.Result<ProjectionType>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
