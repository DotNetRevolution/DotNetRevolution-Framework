using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying
{
    public class Query<TResult> : IQuery<TResult>
        where TResult : class
    {
        public Guid QueryId { get; }

        public Query(Guid queryId)
        {
            Contract.Requires(queryId != Guid.Empty);
            
            QueryId = queryId;
        }
    }
}
