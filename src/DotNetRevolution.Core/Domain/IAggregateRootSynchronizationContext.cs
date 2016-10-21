using DotNetRevolution.Core.Domain.CodeContract;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootSynchronizationContextContract))]
    public interface IAggregateRootSynchronizationContext : IDisposable
    {
        AggregateRootIdentity Identity { get; }

        void Lock();
        Task LockAsync();
        void Unlock();
    }
}