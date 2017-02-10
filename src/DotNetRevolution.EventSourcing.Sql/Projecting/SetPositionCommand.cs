using DotNetRevolution.Core.Hashing;
using DotNetRevolution.EventSourcing.Projecting2;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Sql.Projecting
{
    public class SetPositionCommand
    {
        private readonly SqlCommand _command;
        private readonly SqlParameter _result;

        public SetPositionCommand(ITypeFactory typeFactory,
                                  ProjectionInstance instance)
        {
            Contract.Requires(typeFactory != null);
            Contract.Requires(instance != null);

            _result = new SqlParameter("result", SqlDbType.Int)
            {
                Direction = ParameterDirection.ReturnValue
            };

            _command = CreateSqlCommand(typeFactory, instance);
        }

        public void Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            // execute command
            _command.ExecuteNonQuery();
        }

        public async Task ExecuteAsync(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            await _command.ExecuteNonQueryAsync();
        }
        
        private SqlCommand CreateSqlCommand(ITypeFactory typeFactory, ProjectionInstance instance)
        {
            var sqlCommand = new SqlCommand("[dbo].[SetProjectionPosition]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // set return parameter
            sqlCommand.Parameters.Add(_result);

            var projectionType = instance.ProjectionType;

            // set parameters            
            sqlCommand.Parameters.Add("@projectionTypeId", SqlDbType.Binary, 16).Value = typeFactory.GetHash(projectionType.Type);
            sqlCommand.Parameters.Add("@projectionTypeFullName", SqlDbType.VarChar, 512).Value = projectionType.Type.FullName;
            sqlCommand.Parameters.Add("@eventProviderId", SqlDbType.UniqueIdentifier).Value = instance.EventProviderIdentity.Value;
            sqlCommand.Parameters.Add("@eventProviderVersion", SqlDbType.Int).Value = instance.Position.Value;

            return sqlCommand;
        }
    }
}
