using System;
using DotNetRevolution.Core.Querying;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing.Auditor.Query
{
    public class GetAllAggregateRootTypesQuery : Query<ICollection<GetAllAggregateRootTypesQuery.IResult>>
    {
        public GetAllAggregateRootTypesQuery() 
            : base(Guid.NewGuid())
        {
        }

        public interface IResult
        {
            byte[] AggregateRootId { get; }

            string FullName { get; }
        }
    }
}
