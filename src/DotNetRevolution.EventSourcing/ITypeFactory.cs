using DotNetRevolution.EventSourcing.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
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
