using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventEntryRegistrationContract))]
    public interface IDomainEventEntryRegistration : IDisposable
    {
        IDomainEventEntry Entry { [Pure] get; }
    }
}
