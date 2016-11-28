using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain.CodeContract;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventDispatcherContract))]
    public interface IDomainEventDispatcher
    {
        void Publish(params IDomainEventHandlerContext[] contexts);

        void Publish(params IDomainEvent[] domainEvents);        
    }
}
