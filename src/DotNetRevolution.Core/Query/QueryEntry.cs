using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    public class QueryEntry : IQueryEntry
    {
        public Type QueryType { get; }
        public Type QueryHandlerType { get; }

        public QueryEntry(Type queryType, Type queryHandlerType)
        {
            Contract.Requires(queryType != null);
            Contract.Requires(queryHandlerType != null);

            QueryType = queryType;
            QueryHandlerType = queryHandlerType;
        }        
    }
}
