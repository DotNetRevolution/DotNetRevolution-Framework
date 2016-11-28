using System;
using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandlerContext))]
    internal abstract class CommandHandlerContextContract : ICommandHandlerContext
    {
        public ICommand Command
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                throw new NotImplementedException();
            }
        }

        public MetaCollection Metadata
        {
            get
            {
                Contract.Ensures(Contract.Result<MetaCollection>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
