using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionInitializer<>))]
    internal abstract class ProjectionInitializerContract<TAggregateRoot> : IProjectionInitializer<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Initialize(int batchSize)
        {
            Contract.Requires(batchSize > 0);

            throw new NotImplementedException();
        }
        
        public Task InitializeAsync()
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public Task InitializeAsync(int batchSize)
        {
            Contract.Requires(batchSize > 0);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
