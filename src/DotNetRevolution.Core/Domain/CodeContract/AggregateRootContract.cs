using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRoot))]
    public abstract class AggregateRootContract : IAggregateRoot
    {
        public Identity Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<Identity>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
