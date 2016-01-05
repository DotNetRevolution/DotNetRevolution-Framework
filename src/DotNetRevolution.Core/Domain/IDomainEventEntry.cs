using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventEntryContract))]
    public interface IDomainEventEntry
    {
        Type DomainEventType { [Pure] get; }

        Type DomainEventHandlerType { [Pure] get; }

        IDomainEventHandler DomainEventHandler { [Pure] get; set; }
    }
}
