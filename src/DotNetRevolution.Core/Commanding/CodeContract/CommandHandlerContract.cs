﻿using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandler))]
    internal abstract class CommandHandlerContract : ICommandHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(ICommand command)
        {
            Contract.Requires(command != null);
        }
    }
}
