using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    public class QueryEntry : IQueryEntry
    {
        public Type QueryType { get; }
        public Type QueryHandlerType { get; }
        public IQueryHandler QueryHandler { get; set; }

        public QueryEntry(Type queryType, Type queryHandlerType)
        {
            Contract.Requires(queryType != null);
            Contract.Requires(queryHandlerType != null);

            QueryType = queryType;
            QueryHandlerType = queryHandlerType;
        }

        public QueryEntry(Type queryType, IQueryHandler queryHandler)
        {
            Contract.Requires(queryType != null);
            Contract.Requires(queryHandler != null);

            QueryType = queryType;
            QueryHandler = queryHandler;
            QueryHandlerType = queryHandler.GetType();
        }
    }
}