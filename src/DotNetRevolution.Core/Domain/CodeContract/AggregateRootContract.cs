using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRoot))]
    internal abstract class AggregateRootContract : IAggregateRoot
    {
        public virtual Identity Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<Identity>() != null);

                throw new NotImplementedException();
            }
        }        
    }
}
