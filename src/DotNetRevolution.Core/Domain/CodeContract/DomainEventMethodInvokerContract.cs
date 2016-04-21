using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventMethodInvoker))]
    internal abstract class DomainEventMethodInvokerContract : IDomainEventMethodInvoker
    {
        public void Invoke(object domainEvent)
        {
            Contract.Requires(domainEvent != null);
        }
    }
}
