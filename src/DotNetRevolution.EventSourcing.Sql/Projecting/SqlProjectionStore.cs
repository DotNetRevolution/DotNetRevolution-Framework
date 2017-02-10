using System.Threading.Tasks;
using DotNetRevolution.EventSourcing.Projecting2;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Hashing;
using System.Data.SqlClient;

namespace DotNetRevolution.EventSourcing.Sql.Projecting
{
    public class SqlProjectionStore : IProjectionStore
    {
        private readonly string _connectionString;
        private readonly ITypeFactory _typeFactory;

        public SqlProjectionStore(ITypeFactory typeFactory, 
                                  string connectionString)
        {
            Contract.Requires(typeFactory != null);
            Contract.Requires(string.IsNullOrEmpty(connectionString) == false);

            _connectionString = connectionString;
            _typeFactory = typeFactory;
        }

        public ProjectionInstance GetPosition<TProjection>(EventProviderIdentity eventProviderIdentity) where TProjection : IProjection
        {
            // establish command
            var command = new GetPositionCommand(_typeFactory, new ProjectionType(typeof(TProjection)), eventProviderIdentity);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                return command.Execute(conn);
            }
        }

        public async Task<ProjectionInstance> GetPositionAsync<TProjection>(EventProviderIdentity eventProviderIdentity) where TProjection : IProjection
        {
            // establish command
            var command = new GetPositionCommand(_typeFactory, new ProjectionType(typeof(TProjection)), eventProviderIdentity);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                return await command.ExecuteAsync(conn);
            }
        }

        public void SetPosition(ProjectionInstance instance)
        {
            // establish command
            var command = new SetPositionCommand(_typeFactory, instance);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                command.Execute(conn);
            }
        }

        public Task SetPositionAsync(ProjectionInstance instance)
        {
            // establish command
            var command = new SetPositionCommand(_typeFactory, instance);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                return command.ExecuteAsync(conn);
            }
        }
    }
}
