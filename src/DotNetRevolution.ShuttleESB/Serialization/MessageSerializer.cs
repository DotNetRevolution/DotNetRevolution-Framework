using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Shuttle.Core.Infrastructure;

namespace DotNetRevolution.ShuttleESB.Serialization
{
    public class MessageSerializer : ISerializer
    {
        private readonly Core.Serialization.ISerializer _serializer;
        private readonly Encoding _encoding;
        
        public MessageSerializer(Encoding encoding, 
                                 Core.Serialization.ISerializer serializer)
        {
            Contract.Requires(encoding != null);
            Contract.Requires(serializer != null);

            _serializer = serializer;
            _encoding = encoding;
        }

        public Stream Serialize(object message)
        {
            // implements interface, can't require but can assume / assert
            Contract.Assume(message != null);

            // serialize
            return _serializer.Serialize(message, _encoding);
        }

        public object Deserialize(Type type, Stream stream)
        {
            // implements interface, can't require but can assume / assert
            Contract.Assume(type != null);
            Contract.Assume(stream != null);

            // create an array to store the bytes from the stream
            var bytes = new byte[stream.Length];

            // read bytes into the stream
            stream.Read(bytes, 0, bytes.Length);

            // convert bytes to string
            var data = _encoding.GetString(bytes);
            Contract.Assume(string.IsNullOrWhiteSpace(data) == false);

            // use serializer to deserialize the object
            return _serializer.Deserialize(type, data);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_serializer != null);
            Contract.Invariant(_encoding != null);
        }
    }
}
