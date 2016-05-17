using DotNetRevolution.Core.Base;
using System;

namespace DotNetRevolution.Core.Querying
{
    public class Query<TResult> : IQuery<TResult>
        where TResult : class
    {
        private readonly Guid _id = SequentialGuid.Create();

        public Guid QueryId
        {
            get
            {
                return _id;
            }
        }
    }
}
