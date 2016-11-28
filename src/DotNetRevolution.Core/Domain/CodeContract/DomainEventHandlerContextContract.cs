using DotNetRevolution.Core.Metadata;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandlerContext))]
    internal abstract class DomainEventHandlerContextContract : IDomainEventHandlerContext
    {
        public IDomainEvent DomainEvent
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainEvent>() != null);

                throw new NotImplementedException();
            }
        }

        public MetaCollection Metadata
        {
            get
            {
                Contract.Ensures(Contract.Result<MetaCollection> () != null);

                throw new NotImplementedException();
            }
        }
    }
}
