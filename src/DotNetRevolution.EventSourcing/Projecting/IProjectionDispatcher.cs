using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionDispatcherContract))]
    public interface IProjectionDispatcher
    {
        bool Processed(TransactionIdentity transactionIdentity);

        void Dispatch(params EventProviderTransaction[] eventProviderTransactions);

        void Dispatch(EventProviderTransaction eventProviderTransaction);
    }
}