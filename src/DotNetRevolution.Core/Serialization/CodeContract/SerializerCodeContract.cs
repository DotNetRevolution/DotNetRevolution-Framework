using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace DotNetRevolution.Core.Serialization.CodeContract
{
    [ContractClassFor(typeof(ISerializer))]
    public abstract class SerializerCodeContract : ISerializer
    {
        public string Serialize(object item)
        {
            Contract.Requires(item != null);
            Contract.Ensures(Contract.Result<string>() != null);

            throw new NotImplementedException();
        }

        public Stream Serialize(object item, Encoding encoding)
        {
            Contract.Requires(item != null);
            Contract.Requires(encoding != null);
            Contract.Ensures(Contract.Result<Stream>() != null);

            throw new NotImplementedException();
        }

        public object Deserialize(Type type, string data)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(data));
            Contract.Ensures(Contract.Result<object>() != null);

            throw new NotImplementedException();
        }
    }
}
