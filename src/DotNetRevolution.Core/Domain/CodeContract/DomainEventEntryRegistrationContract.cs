using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventEntryRegistration))]
    internal abstract class DomainEventEntryRegistrationContract : IDomainEventEntryRegistration
    {
        public IDomainEventEntry Entry
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainEventEntry>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract void Dispose();
    }
}
