using DotNetRevolution.Core.Hashing.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Hashing
{
    [ContractClass(typeof(TypeFactoryContract))]
    public interface ITypeFactory
    {
        [Pure]
        byte[] GetHash(Type type);

        [Pure]
        Type GetType(byte[] hash);        
    }
}
