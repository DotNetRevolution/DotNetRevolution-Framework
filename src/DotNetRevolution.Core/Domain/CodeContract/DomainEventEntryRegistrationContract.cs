using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventEntryRegistration))]
    public abstract class DomainEventEntryRegistrationContract : IDomainEventEntryRegistration
    {
        public Guid Id
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);

                throw new NotImplementedException();
            }
        }

        public abstract void Dispose();
    }
}
