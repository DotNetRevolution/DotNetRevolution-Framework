using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Querying.CodeContract;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryHandlerContract))]
    public interface IQueryHandler
    {
        bool Reusable { [Pure] get; }

        [Pure]
        TResult Handle<TResult>(IQuery<TResult> query) where TResult : class;

        [Pure]
        Task<TResult> HandleAsync<TResult>(IQuery<TResult> query) where TResult : class;
    }
}
