using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionInitializer))]
    internal abstract class ProjectionInitializerContract : IProjectionInitializer
    {
        EventProviderTransaction IProjectionInitializer.Initialize<TAggregateRoot>()
        {
            throw new NotImplementedException();
        }

        EventProviderTransaction IProjectionInitializer.Initialize<TAggregateRoot>(int batchSize)
        {
            Contract.Requires(batchSize > 0);

            throw new NotImplementedException();
        }
        
        Task<EventProviderTransaction> IProjectionInitializer.InitializeAsync<TAggregateRoot>()
        {
            Contract.Ensures(Contract.Result<Task<EventProviderTransaction>>() != null);

            throw new NotImplementedException();
        }

        Task<EventProviderTransaction> IProjectionInitializer.InitializeAsync<TAggregateRoot>(int batchSize)
        {
            Contract.Requires(batchSize > 0);
            Contract.Ensures(Contract.Result<Task<EventProviderTransaction>>() != null);

            throw new NotImplementedException();
        }
    }
}
