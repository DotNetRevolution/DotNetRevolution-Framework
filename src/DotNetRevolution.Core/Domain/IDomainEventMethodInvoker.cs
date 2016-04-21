using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventMethodInvokerContract))]
    public interface IDomainEventMethodInvoker
    {
        void Invoke(object domainEvent);
    }
}
