using System;
using DotNetRevolution.Core.Querying;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class Query1 : Query<Query1.Result>
    {
        public Query1(Guid queryId)
            : base(queryId)
        {
            Contract.Requires(queryId != Guid.Empty);
        }

        public class Result
        {
        }
    }
}
