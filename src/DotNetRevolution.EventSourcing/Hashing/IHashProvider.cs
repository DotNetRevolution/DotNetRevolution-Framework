using DotNetRevolution.EventSourcing.Hashing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(HashProviderContract))]
    public interface IHashProvider
    {
        [Pure]
        byte[] GetHash(string value);
    }
}
