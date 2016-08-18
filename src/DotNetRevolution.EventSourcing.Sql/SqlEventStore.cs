using DotNetRevolution.Core.Serialization;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;
using System.Data.SqlClient;
using System.Text;
using System;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing.Sql
{
    public class SqlEventStore : EventStore
    {
        private readonly SqlSerializer _serializer;
        private readonly ITypeFactory _typeFactory;
        private readonly string _connectionString;        

        public SqlEventStore(IUsernameProvider usernameProvider,
                             ISerializer serializer,
                             ITypeFactory typeFactory,
                             Encoding encoding,
                             string connectionString)
            : base(usernameProvider, serializer)
        {
            Contract.Requires(encoding != null);
            Contract.Requires(serializer != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(usernameProvider != null);
            Contract.Requires(string.IsNullOrEmpty(connectionString) == false);
            
            _connectionString = connectionString;
            _serializer = new SqlSerializer(encoding, serializer, typeFactory);
            _typeFactory = typeFactory;
        }

        protected override EventStream GetEventStream(EventProviderType eventProviderType, Identity identity)
        {
            // establish command
            var command = new GetDomainEventsCommand(_serializer, _typeFactory, eventProviderType, identity);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                command.Execute(conn);
            }

            return command.GetResults();
        }

        protected override void CommitTransaction(string username, EventProviderTransaction transaction, IReadOnlyCollection<EventStreamRevision> uncommittedRevisions)
        {
            // establish command
            var command = new CommitTransactionCommand(_serializer,
                _typeFactory,
                username,
                transaction,
                uncommittedRevisions);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                command.Execute(conn);
            }            
        }
    }
}
