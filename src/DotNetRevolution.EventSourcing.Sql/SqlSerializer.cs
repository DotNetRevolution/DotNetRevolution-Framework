using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Helper;
using DotNetRevolution.Core.Serialization;
using DotNetRevolution.EventSourcing.Snapshotting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlSerializer
    {
        private static readonly Encoding _encoding = Encoding.UTF8;

        private readonly ISerializer _serializer;

        public SqlSerializer(ISerializer serializer)
        {
            Contract.Requires(serializer != null);

            _serializer = serializer;
        }
        
        public IReadOnlyCollection<IDomainEvent> DeserializeDomainEvents(Collection<SqlDomainEvent> sqlDomainEvents)
        {
            return new ReadOnlyCollection<IDomainEvent>(sqlDomainEvents
                .OrderBy(x => x.EventProviderVersion)
                .ThenBy(x => x.Sequence)
                .Select(x => DeserializeDomainEvent(x.EventType, x.Data))
                .ToList());
        }

        public Snapshot DeserializeSnapshot(SqlSnapshot sqlSnapshot)
        {
            Contract.Requires(sqlSnapshot != null);

            // deserialize snapshot data
            var deserializedObject = _serializer.Deserialize(TypeHelper.Find(sqlSnapshot.FullName), sqlSnapshot.Data, _encoding);

            // make sure object was deserialized
            if (deserializedObject != null)
            {
                return new Snapshot(deserializedObject);
            }

            throw new SnapshotSerializationException("Could not deserialize snapshot data");
        }

        public IDomainEvent DeserializeDomainEvent(string domainEventType, byte[] data)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(domainEventType) == false);
            Contract.Requires(data != null);
            Contract.Requires(data.Length > 0);
            Contract.Ensures(Contract.Result<IDomainEvent>() != null);

            // deserialize domain event
            var deserializedObject = _serializer.Deserialize(TypeHelper.Find(domainEventType), data, _encoding);

            // make sure deserialization was successful
            if (deserializedObject == null)
            {
                throw new DomainEventSerializationException(string.Format("Could not deserialize {0}", domainEventType));
            }

            // make sure deserialized object is a domain event
            if (deserializedObject is IDomainEvent)
            {
                return deserializedObject as IDomainEvent;
            }

            // error deserializing domain event
            throw new DomainEventSerializationException("Deserialized object is not a domain event.");
        }

        public byte[] SerializeObject(object objectToSerialize)
        {
            Contract.Requires(objectToSerialize != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length > 0);

            // serialize object to bytes for storage
            var bytes = _encoding.GetBytes(_serializer.Serialize(objectToSerialize));
            Contract.Assume(bytes != null);
            Contract.Assume(bytes.Length > 0);

            return bytes;
        }
    }
}
