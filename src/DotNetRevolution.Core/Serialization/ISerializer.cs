using System;
using System.Diagnostics.Contracts;
using System.IO;
using DotNetRevolution.Core.Serialization.CodeContract;

namespace DotNetRevolution.Core.Serialization
{
    [ContractClass(typeof(SerializerCodeContract))]
    public interface ISerializer
    {
        string Serialize(object item);
        Stream Serialize(object item, System.Text.Encoding encoding);
        object Deserialize(Type type, string data);
        object Deserialize(Type type, Stream data, System.Text.Encoding encoding);
        object Deserialize(Type type, byte[] data, System.Text.Encoding encoding);
    }
}
