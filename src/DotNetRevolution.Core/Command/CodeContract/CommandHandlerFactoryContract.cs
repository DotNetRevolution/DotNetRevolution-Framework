﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command.CodeContract
{
    [ContractClassFor(typeof(ICommandHandlerFactory))]
    public abstract class CommandHandlerFactoryContract : ICommandHandlerFactory
    {
        public ICommandCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommandCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public ICommandHandler GetHandler(object command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            throw new NotImplementedException();
        }
    }
}
