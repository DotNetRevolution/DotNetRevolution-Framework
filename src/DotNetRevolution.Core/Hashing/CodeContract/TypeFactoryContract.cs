using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Hashing.CodeContract
{
    [ContractClassFor(typeof(ITypeFactory))]
    internal abstract class TypeFactoryContract : ITypeFactory
    {
        public byte[] GetHash(Type type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            throw new NotImplementedException();
        }

        public Type GetType(byte[] hash)
        {
            Contract.Requires(hash != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            throw new NotImplementedException();
        }
    }
}
