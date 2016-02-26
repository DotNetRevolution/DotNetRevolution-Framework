using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DotNetRevolution.Core.Serialization
{
    public class BinarySerializer : ISerializer
    {
        private readonly BinaryFormatter _serializer;

        public BinarySerializer()
        {
            _serializer = new BinaryFormatter();
        }

        public object Deserialize(Type type, string data)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object item)
        {
            throw new NotImplementedException();
        }

        public Stream Serialize(object item, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
