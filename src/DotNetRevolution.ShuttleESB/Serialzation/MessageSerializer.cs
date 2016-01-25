using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Shuttle.Core.Infrastructure;

namespace DotNetRevolution.ShuttleESB.Serialzation
{
    public class MessageSerializer : ISerializer
    {
        private readonly Core.Serialization.ISerializer _serializer;

        public MessageSerializer(Core.Serialization.ISerializer serializer)
        {
            Contract.Requires(serializer != null);

            _serializer = serializer;
        }

        public Stream Serialize(object message)
        {
            // implements interface, can't require but can assume / assert
            Contract.Assume(message != null);

            // serialize
            return _serializer.Serialize(message, Encoding.UTF8);
        }

        public object Deserialize(Type type, Stream stream)
        {
            // implements interface, can't require but can assume / assert
            Contract.Assume(stream != null);

            // create an array to store the bytes from the stream
            var bytes = new byte[stream.Length];

            // read bytes into the stream
            stream.Read(bytes, 0, bytes.Length);

            // use serializer to deserialize the object
            return _serializer.Deserialize(type, Encoding.UTF8.GetString(bytes));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_serializer != null);
        }
    }
}
