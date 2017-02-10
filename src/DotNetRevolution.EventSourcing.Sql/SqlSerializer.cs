using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.Core.Serialization;
using DotNetRevolution.EventSourcing.Snapshotting;
using DotNetRevolution.EventSourcing.Sql.ReadDomainEvent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlSerializer
    {
        private readonly ISerializer _serializer;
        private readonly ITypeFactory _typeFactory;
        private readonly Encoding _encoding;

        public SqlSerializer(Encoding encoding,
                             ISerializer serializer,
                             ITypeFactory typeFactory)
        {
            Contract.Requires(encoding != null);
            Contract.Requires(serializer != null);
            Contract.Requires(typeFactory != null);

            _encoding = encoding;
            _serializer = serializer;
            _typeFactory = typeFactory;
        }
        
        public IReadOnlyCollection<IDomainEvent> DeserializeDomainEvents(Collection<SqlDomainEvent> sqlDomainEvents)
        {
            return new ReadOnlyCollection<IDomainEvent>(sqlDomainEvents
                .OrderBy(x => x.EventProviderVersion)
                .ThenBy(x => x.Sequence)
                .Select(x => Deserialize<IDomainEvent>(x.EventTypeId, x.Data))
                .ToList());
        }

        public Snapshot DeserializeSnapshot(SqlSnapshot sqlSnapshot)
        {
            Contract.Requires(sqlSnapshot != null);

            // deserialize snapshot data
            var deserializedObject = _serializer.Deserialize(_typeFactory.GetType(sqlSnapshot.TypeId), sqlSnapshot.Data, _encoding);

            // make sure object was deserialized
            if (deserializedObject != null)
            {
                return new Snapshot(deserializedObject);
            }

            throw new EventStoreSerializationException("Could not deserialize snapshot data");
        }

        internal T Deserialize<T>(byte[] data) where T : class
        {
            Contract.Requires(data != null);
            Contract.Requires(data.Length > 0);
            Contract.Ensures(Contract.Result<ICommand>() != null);

            // deserialize data
            var deserializedObject = _serializer.Deserialize(typeof(T), data, _encoding);

            // make sure deserialization was successful
            if (deserializedObject == null)
            {
                throw new EventStoreSerializationException($"Could not deserialize type {typeof(T).FullName}");
            }

            var obj = deserializedObject as T;

            // make sure deserialized object is of type T
            if (obj == null)
            {
                // error, throw exception
                throw new EventStoreSerializationException($"Deserialized object is not of type {typeof(T).FullName}.");
            }

            return obj;
        }

        internal T Deserialize<T>(byte[] typeId, byte[] data) where T : class
        {
            Contract.Requires(typeId != null);
            Contract.Requires(typeId.Length > 0);
            Contract.Requires(data != null);
            Contract.Requires(data.Length > 0);
            Contract.Ensures(Contract.Result<ICommand>() != null);

            // deserialize data
            var deserializedObject = _serializer.Deserialize(_typeFactory.GetType(typeId), data, _encoding);

            // make sure deserialization was successful
            if (deserializedObject == null)
            {
                throw new EventStoreSerializationException($"Could not deserialize type with hash {typeId}");
            }

            var obj = deserializedObject as T;

            // make sure deserialized object is of type T
            if (obj == null)
            {
                // error, throw exception
                throw new EventStoreSerializationException($"Deserialized object is not of type {typeof(T).FullName}.");
            }

            return obj;
        }

        internal byte[] SerializeObject(object objectToSerialize)
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
