using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Query.CodeContract;

namespace DotNetRevolution.Core.Query
{
    [ContractClass(typeof(QueryDispatcherContract))]
    public interface IQueryDispatcher
    {
        TResult Dispatch<TResult>(object query);
    }
}