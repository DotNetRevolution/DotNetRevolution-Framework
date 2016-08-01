using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.GuidGeneration.CodeContract
{
    [ContractClassFor(typeof(IGuidGenerator))]
    internal abstract class GuidGeneratorContract : IGuidGenerator
    {
        public Guid Create()
        {
            Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);

            throw new NotImplementedException();
        }
    }
}
