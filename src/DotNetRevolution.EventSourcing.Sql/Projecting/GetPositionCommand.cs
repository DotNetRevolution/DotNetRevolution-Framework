using DotNetRevolution.Core.Hashing;
using DotNetRevolution.EventSourcing.Projecting2;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Sql.Projecting
{
    public class GetPositionCommand
    {
        private readonly SqlCommand _command;
        private readonly ProjectionType _projectionType;
        private readonly EventProviderIdentity _eventProviderIdentity;

        public GetPositionCommand(ITypeFactory typeFactory,
                                  ProjectionType projectionType,
                                  EventProviderIdentity eventProviderIdentity)
        {
            Contract.Requires(typeFactory != null);
            Contract.Requires(projectionType != null);
            Contract.Requires(eventProviderIdentity != null);

            _projectionType = projectionType;
            _eventProviderIdentity = eventProviderIdentity;

            _command = CreateSqlCommand(typeFactory, projectionType, eventProviderIdentity);
        }

        public ProjectionInstance Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = _command.ExecuteReader())
            {
                // execute command
                return ExecuteSqlCommand(dataReader);
            }
        }
        public async Task<ProjectionInstance> ExecuteAsync(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = await _command.ExecuteReaderAsync())
            {
                // execute command
                return ExecuteSqlCommand(dataReader);
            }
        }

        private ProjectionInstance ExecuteSqlCommand(SqlDataReader dataReader)
        {
            Contract.Requires(dataReader != null);

            // throw exception if no rows
            if (dataReader.HasRows == false)
            {
                throw new ProjectionNotFoundException();
            }

            return new ProjectionInstance(_projectionType, _eventProviderIdentity, dataReader.GetInt32(0));
        }

        private SqlCommand CreateSqlCommand(ITypeFactory typeFactory, ProjectionType projectionType, EventProviderIdentity eventProviderIdentity)
        {
            var sqlCommand = new SqlCommand("[dbo].[GetProjectionPosition]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // set parameters
            sqlCommand.Parameters.Add("@projectionId", SqlDbType.UniqueIdentifier).Value = typeFactory.GetHash(projectionType.Type);
            sqlCommand.Parameters.Add("@eventProviderId", SqlDbType.UniqueIdentifier).Value = eventProviderIdentity.Value;

            return sqlCommand;
        }
    }
}
