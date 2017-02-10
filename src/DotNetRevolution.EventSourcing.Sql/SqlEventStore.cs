using DotNetRevolution.Core.Serialization;
using System.Diagnostics.Contracts;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.EventSourcing.Sql.ReadDomainEvent;
using DotNetRevolution.EventSourcing.Sql.CommitTransaction;
using DotNetRevolution.EventSourcing.Sql.ReadTransaction;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing.Sql
{
    public class SqlEventStore : EventStore
    {
        private readonly SqlSerializer _serializer;
        private readonly ITypeFactory _typeFactory;
        private readonly string _connectionString;        

        public SqlEventStore(ISerializer serializer,
                             ITypeFactory typeFactory,
                             Encoding encoding,
                             string connectionString)
            : base(serializer)
        {
            Contract.Requires(encoding != null);
            Contract.Requires(serializer != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(string.IsNullOrEmpty(connectionString) == false);
            
            _connectionString = connectionString;
            _serializer = new SqlSerializer(encoding, serializer, typeFactory);
            _typeFactory = typeFactory;
        }

        protected override EventStream GetEventStream(AggregateRootType eventProviderType, AggregateRootIdentity identity)
        {
            // establish command
            var command = new GetDomainEventsCommand(_serializer, _typeFactory, eventProviderType, identity);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                return command.Execute(conn);
            }
        }

        protected override async Task<EventStream> GetEventStreamAsync(AggregateRootType eventProviderType, AggregateRootIdentity identity)
        {
            // establish command
            var command = new GetDomainEventsCommand(_serializer, _typeFactory, eventProviderType, identity);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                await conn.OpenAsync();

                // execute
                return await command.ExecuteAsync(conn);
            }
        }

        protected override void CommitTransaction(EventProviderTransaction transaction)
        {
            // establish command
            var command = new CommitTransactionCommand(_serializer,
                _typeFactory,
                transaction,
                transaction.Revisions);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                command.Execute(conn);
            }
        }

        protected override async Task CommitTransactionAsync(EventProviderTransaction transaction)
        {
            // establish command
            var command = new CommitTransactionCommand(_serializer,
                _typeFactory,
                transaction,
                transaction.Revisions);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                await conn.OpenAsync();

                // execute
                await command.ExecuteAsync(conn);
            }
        }

        protected override ICollection<EventProviderTransactionCollection> GetTransactions(AggregateRootType aggregateRootType, int eventProvidersToSkip, int eventProvidersToTake)
        {
            // establish command
            var command = new GetTransactionsByAggregateRootTypeCommand(_serializer, _typeFactory, aggregateRootType, eventProvidersToSkip, eventProvidersToTake);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                return command.Execute(conn);
            }
        }

        protected override Task<ICollection<EventProviderTransactionCollection>> GetTransactionsAsync(AggregateRootType aggregateRootType, int eventProvidersToSkip, int eventProvidersToTake)
        {
            // establish command
            var command = new GetTransactionsByAggregateRootTypeCommand(_serializer, _typeFactory, aggregateRootType, eventProvidersToSkip, eventProvidersToTake);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.OpenAsync();

                // execute
                return command.ExecuteAsync(conn);
            }
        }
    }
}
