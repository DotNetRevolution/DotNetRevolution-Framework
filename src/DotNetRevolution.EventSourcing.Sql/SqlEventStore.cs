using DotNetRevolution.Core.Serialization;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;
using System.Data.SqlClient;
using DotNetRevolution.EventSourcing.Snapshotting;
using DotNetRevolution.EventSourcing.AggregateRoot;
using System.Text;
using DotNetRevolution.Core.GuidGeneration;

namespace DotNetRevolution.EventSourcing.Sql
{
    public class SqlEventStore : EventStore
    {
        private readonly GetSnapshotIfPolicySatisfiedDelegate _getSnapshotIfPolicySatisfiedDelegate;

        private readonly SqlSerializer _serializer;
        private readonly ITypeFactory _typeFactory;
        private readonly string _connectionString;
        private readonly IGuidGenerator _guidGenerator;

        public SqlEventStore(IAggregateRootProcessorFactory eventStreamProcessorProvider,
                             ISnapshotPolicyFactory snapshotPolicyProvider,
                             ISnapshotProviderFactory snapshotProviderFactory,
                             IUsernameProvider usernameProvider,
                             ISerializer serializer,
                             ITypeFactory typeFactory,
                             Encoding encoding,
                             string connectionString)
            : base(eventStreamProcessorProvider, snapshotPolicyProvider, snapshotProviderFactory, usernameProvider, serializer)
        {
            Contract.Requires(eventStreamProcessorProvider != null);
            Contract.Requires(encoding != null);
            Contract.Requires(snapshotPolicyProvider != null);
            Contract.Requires(serializer != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(usernameProvider != null);
            Contract.Requires(string.IsNullOrEmpty(connectionString) == false);
            
            _connectionString = connectionString;
            _serializer = new SqlSerializer(encoding, serializer, typeFactory);
            _typeFactory = typeFactory;

            _guidGenerator = new SequentialGuidGenerator(SequentialGuidType.SequentialAtEnd);

            _getSnapshotIfPolicySatisfiedDelegate = new GetSnapshotIfPolicySatisfiedDelegate(GetSnapshotIfPolicySatisfied);
        }

        protected override EventStream GetDomainEvents(EventProviderType eventProviderType, Identity identity, out Identity globalIdentity, out EventProviderVersion version, out EventProviderDescriptor eventProviderDescriptor, out Snapshot snapshot)
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

            return command.GetResults(out globalIdentity, out eventProviderDescriptor, out version, out snapshot);
        }

        protected override void CommitTransaction(string username, EventProviderTransaction transaction)
        {
            // establish command
            var command = new CommitTransactionCommand(_serializer, 
                _getSnapshotIfPolicySatisfiedDelegate,
                _typeFactory,
                username,
                transaction,
                _guidGenerator);

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
