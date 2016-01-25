using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using DotNetRevolution.Core.Serialization.CodeContract;

namespace DotNetRevolution.Core.Serialization
{
    [ContractClass(typeof(SerializerCodeContract))]
    public interface ISerializer
    {
        string Serialize(object item);
        Stream Serialize(object item, Encoding encoding);
        object Deserialize(Type type, string data);
    }
}
