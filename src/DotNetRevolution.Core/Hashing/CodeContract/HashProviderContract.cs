using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Hashing.CodeContract
{
    [ContractClassFor(typeof(IHashProvider))]
    internal abstract class HashProviderContract : IHashProvider
    {
        public byte[] GetHash(string value)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(value) == false);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            throw new NotImplementedException();
        }
    }
}
