using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandlerFactory))]
    internal abstract class CommandHandlerFactoryContract : ICommandHandlerFactory
    {
        public ICommandCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommandCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public ICommandHandler GetHandler(Type commandType)
        {
            Contract.Requires(commandType != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            throw new NotImplementedException();
        }
    }
}
