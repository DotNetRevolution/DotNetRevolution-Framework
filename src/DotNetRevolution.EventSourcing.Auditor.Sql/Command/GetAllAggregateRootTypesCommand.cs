using DotNetRevolution.EventSourcing.Auditor.Query;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Auditor.Sql.Command
{
    internal class GetAllAggregateRootTypesCommand
    {
        private readonly SqlCommand _command;

        private readonly SqlParameter _result;

        public GetAllAggregateRootTypesCommand()
        {            
            _command = CreateSqlCommand();
        }

        public ICollection<GetAllAggregateRootTypesQuery.IResult> Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = _command.ExecuteReader())
            {
                // execute command
                ExecuteSqlCommand(dataReader);
            }

            return GetResult();
        }

        public async Task<ICollection<GetAllAggregateRootTypesQuery.IResult>> ExecuteAsync(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = await _command.ExecuteReaderAsync())
            {
                // execute command
                _sqlDomainEvents = ExecuteSqlCommand(dataReader);
            }

            return GetResult();
        }

        private SqlCommand CreateSqlCommand()
        {
            var sqlCommand = new SqlCommand("[dbo].[GetAllAggregateRootTypes]");
            sqlCommand.CommandType = CommandType.StoredProcedure;
                        
            return sqlCommand;
        }

        private EventStream GetResult()
        {
            
        }        
    }
}
