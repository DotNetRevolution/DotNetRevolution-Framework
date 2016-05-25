using DotNetRevolution.Core.Serialization;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;
using System.Data.SqlClient;
using DotNetRevolution.EventSourcing.Snapshotting;
using DotNetRevolution.EventSourcing.AggregateRoot;

namespace DotNetRevolution.EventSourcing.Sql
{
    public class SqlEventStore : EventStore
    {
        private readonly GetSnapshotIfPolicySatisfiedDelegate _getSnapshotIfPolicySatisfiedDelegate;

        private readonly SqlSerializer _serializer;
        private readonly string _connectionString;

        public SqlEventStore(IAggregateRootProcessorFactory eventStreamProcessorProvider,
                             ISnapshotPolicyFactory snapshotPolicyProvider,
                             ISnapshotProviderFactory snapshotProviderFactory,
                             ISerializer serializer,
                             string connectionString)
            : base(eventStreamProcessorProvider, snapshotPolicyProvider, snapshotProviderFactory, serializer)
        {
            Contract.Requires(eventStreamProcessorProvider != null);
            Contract.Requires(snapshotPolicyProvider != null);
            Contract.Requires(serializer != null);
            Contract.Requires(string.IsNullOrEmpty(connectionString) == false);
            
            _connectionString = connectionString;
            _serializer = new SqlSerializer(serializer);
            
            _getSnapshotIfPolicySatisfiedDelegate = new GetSnapshotIfPolicySatisfiedDelegate(GetSnapshotIfPolicySatisfied);
        }

        protected override EventStream GetDomainEvents(EventProviderType eventProviderType, Identity identity, out EventProviderVersion version, out EventProviderDescriptor eventProviderDescriptor, out Snapshot snapshot)
        {
            // establish command
            var command = new GetDomainEventsCommand(_serializer, eventProviderType, identity);

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // connection needs to be open before executing
                conn.Open();

                // execute
                command.Execute(conn);
            }

            return command.GetResults(out eventProviderDescriptor, out version, out snapshot);
        }

        protected override void CommitTransaction(Transaction transaction)
        {
            // establish command
            var command = new CommitTransactionCommand(_serializer, 
                _getSnapshotIfPolicySatisfiedDelegate,
                transaction);

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
