using System;
using DotNetRevolution.Core.Querying;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class Query2 : Query<Query2.Result>
    {
        public Query2(Guid queryId) 
            : base(queryId)
        {
            Contract.Requires(queryId != Guid.Empty);
        }

        public class Result
        {
        }
    }
}
