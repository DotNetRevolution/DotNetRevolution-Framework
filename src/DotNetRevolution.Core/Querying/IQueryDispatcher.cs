using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Querying.CodeContract;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryDispatcherContract))]
    public interface IQueryDispatcher
    {
        TResult Dispatch<TResult>(IQuery<TResult> query) where TResult : class;
    }
}
