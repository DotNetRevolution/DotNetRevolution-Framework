using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventEntry))]
    public abstract class DomainEventEntryContract : IDomainEventEntry
    {
        public Type DomainEventType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public Type DomainEventHandlerType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract IDomainEventHandler DomainEventHandler { get; set; }
    }
}
