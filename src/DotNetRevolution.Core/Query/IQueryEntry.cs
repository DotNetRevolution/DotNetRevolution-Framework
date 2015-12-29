using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Query.CodeContract;

namespace DotNetRevolution.Core.Query
{
    [ContractClass(typeof(QueryEntryContract))]
    public interface IQueryEntry
    {
        Type QueryType { [Pure] get; }

        Type QueryHandlerType { [Pure] get; }

        IQueryHandler QueryHandler { [Pure] get; set; }
    }
}