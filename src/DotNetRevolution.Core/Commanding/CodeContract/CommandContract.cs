using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommand))]
    internal abstract class CommandContract : ICommand
    {
        public Guid CommandId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);

                throw new NotImplementedException();
            }
        }
    }
}
