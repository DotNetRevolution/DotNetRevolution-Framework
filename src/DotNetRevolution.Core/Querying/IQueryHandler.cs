using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Querying.CodeContract;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryHandlerContract))]
    public interface IQueryHandler
    {
        bool Reusable { [Pure] get; }

        [Pure]
        TResult Handle<TResult>(IQuery<TResult> query) where TResult : class;
    }
    
    [ContractClass(typeof(QueryHandlerContract<,>))]
    public interface IQueryHandler<in TQuery, out TResult> : IQueryHandler
        where TQuery : IQuery<TResult> 
        where TResult : class
    {
        [Pure]
        TResult Handle(TQuery query);
    }
}
