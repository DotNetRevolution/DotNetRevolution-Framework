using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Query.CodeContract;

namespace DotNetRevolution.Core.Query
{
    [ContractClass(typeof(QueryHandlerContract))]
    public interface IQueryHandler
    {
        bool Reusable { [Pure] get; }

        [Pure]
        object Handle(object query);
    }
    
    [ContractClass(typeof(QueryHandlerContract<,>))]
    public interface IQueryHandler<in TQuery, out TResult> : IQueryHandler
        where TQuery : class 
        where TResult : class
    {
        [Pure]
        TResult Handle(TQuery query);
    }
}