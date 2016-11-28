using DotNetRevolution.Core.Metadata;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandlerContext<>))]
    internal abstract class DomainEventHandlerContextContract<TDomainEvent> : IDomainEventHandlerContext<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public TDomainEvent DomainEvent
        {
            get
            {
                Contract.Ensures(Contract.Result<TDomainEvent>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract MetaCollection Metadata { get; }

        IDomainEvent IDomainEventHandlerContext.DomainEvent
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
