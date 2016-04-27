using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Querying.CodeContract;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryEntryContract))]
    public interface IQueryEntry
    {
        Type QueryType { [Pure] get; }

        Type QueryHandlerType { [Pure] get; }
    }
}
