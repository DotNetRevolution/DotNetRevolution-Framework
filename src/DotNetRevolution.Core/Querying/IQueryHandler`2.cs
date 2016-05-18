using DotNetRevolution.Core.Querying.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryHandlerContract<,>))]
    public interface IQueryHandler<in TQuery, out TResult> : IQueryHandler
        where TQuery : IQuery<TResult>
        where TResult : class
    {
        [Pure]
        TResult Handle(TQuery query);
    }
}
