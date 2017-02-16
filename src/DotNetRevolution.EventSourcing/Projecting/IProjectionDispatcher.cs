using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionDispatcherContract))]
    public interface IProjectionDispatcher
    {
        [Pure]
        bool Processed(TransactionIdentity transactionIdentity);

        void Dispatch(params EventProviderTransaction[] eventProviderTransactions);

        void Dispatch(EventProviderTransaction eventProviderTransaction);
    }
}