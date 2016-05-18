using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    [ContractClass(typeof(DomainEventHandlerContract))]
    public interface IDomainEventHandler
    {
        bool Reusable { [Pure] get; }
        void Handle(IDomainEvent domainEvent);
    }
}
