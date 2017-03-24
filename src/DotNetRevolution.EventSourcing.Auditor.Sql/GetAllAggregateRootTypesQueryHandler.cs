using DotNetRevolution.Core.Querying;
using DotNetRevolution.EventSourcing.Auditor.Query;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Auditor.Sql
{
    public class GetAllAggregateRootTypesQueryHandler : QueryHandler<GetAllAggregateRootTypesQuery, ICollection<GetAllAggregateRootTypesQuery.IResult>>
    {
        private readonly string _connString;

        public GetAllAggregateRootTypesQueryHandler(string connString)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(connString) == false);

            _connString = connString;
        }

        public override ICollection<GetAllAggregateRootTypesQuery.IResult> Handle(GetAllAggregateRootTypesQuery query)
        {
            using (var conn = new SqlConnection(_connString))
            {
                var cmd = new SqlCommand()
            }
        }

        public override Task<ICollection<GetAllAggregateRootTypesQuery.IResult>> HandleAsync(GetAllAggregateRootTypesQuery query)
        {
            using (var conn = new SqlConnection(_connString))
            {
                var cmd = new SqlCommand()
            }
        }
    }
}
