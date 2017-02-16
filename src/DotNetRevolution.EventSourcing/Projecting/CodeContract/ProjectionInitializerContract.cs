using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionInitializer))]
    internal abstract class ProjectionInitializerContract : IProjectionInitializer
    {
        void IProjectionInitializer.Initialize<TAggregateRoot>()
        {
            throw new NotImplementedException();
        }

        void IProjectionInitializer.Initialize<TAggregateRoot>(int batchSize)
        {
            Contract.Requires(batchSize > 0);

            throw new NotImplementedException();
        }
        
        Task IProjectionInitializer.InitializeAsync<TAggregateRoot>()
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        Task IProjectionInitializer.InitializeAsync<TAggregateRoot>(int batchSize)
        {
            Contract.Requires(batchSize > 0);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
