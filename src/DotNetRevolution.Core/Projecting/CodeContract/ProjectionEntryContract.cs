using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionEntry))]
    internal abstract class ProjectionEntryContract : IProjectionEntry
    {
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

        public Type ProjectionType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
