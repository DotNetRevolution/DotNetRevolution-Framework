﻿using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandler))]
    internal abstract class DomainEventHandlerContract : IDomainEventHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(IDomainEventHandlerContext context)
        {
            Contract.Requires(context != null);
        }
    }
}
