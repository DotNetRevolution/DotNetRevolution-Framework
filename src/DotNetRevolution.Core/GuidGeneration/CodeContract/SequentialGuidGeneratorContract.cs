using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.GuidGeneration.CodeContract
{
    [ContractClassFor(typeof(SequentialGuidGenerator))]
    internal abstract class SequentialGuidGeneratorContract : SequentialGuidGenerator
    {
        protected override byte[] OrderBytes(byte[] timestampBytes, byte[] randomBytes)
        {
            Contract.Requires(timestampBytes != null);
            Contract.Requires(timestampBytes.Length == 8);
            Contract.Requires(randomBytes != null);
            Contract.Requires(randomBytes.Length == 10);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == 16);

            throw new NotImplementedException();
        }
    }
}
