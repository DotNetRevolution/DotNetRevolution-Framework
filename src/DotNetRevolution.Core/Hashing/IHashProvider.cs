using DotNetRevolution.Core.Hashing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Hashing
{
    [ContractClass(typeof(HashProviderContract))]
    public interface IHashProvider
    {
        [Pure]
        byte[] GetHash(string value);
    }
}
