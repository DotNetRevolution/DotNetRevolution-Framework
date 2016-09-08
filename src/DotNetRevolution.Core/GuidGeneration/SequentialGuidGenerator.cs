// https://github.com/jhtodd/SequentialGuid

using DotNetRevolution.Core.GuidGeneration.CodeContract;
using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace DotNetRevolution.Core.GuidGeneration
{
    [ContractClass(typeof(SequentialGuidGeneratorContract))]
    public abstract class SequentialGuidGenerator : IGuidGenerator
    { 
        private static readonly RNGCryptoServiceProvider RandomGenerator = new RNGCryptoServiceProvider();
     
        protected abstract byte[] OrderBytes(byte[] timestampBytes, byte[] randomBytes);
        
        public Guid Create()
        {
            // start with 10 bytes of cryptographically strong random data
            byte[] randomBytes = new byte[10];
            RandomGenerator.GetBytes(randomBytes);
            
            // get timestamp bytes
            byte[] timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L);

            // converting from an Int64, reverse on little-endian systems
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            // order bytes according to implementation
            byte[] guidBytes = OrderBytes(timestampBytes, randomBytes);
            
            // convert bytes to guid
            var result = new Guid(guidBytes);
            Contract.Assume(result != Guid.Empty);

            // return new sequential guid
            return result;
        }
    }
}
