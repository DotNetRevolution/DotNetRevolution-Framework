using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Querying.CodeContract;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryDispatcherContract))]
    public interface IQueryDispatcher
    {
        TResult Dispatch<TResult>(IQuery<TResult> query) where TResult : class;

        Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query) where TResult : class;
    }
}
