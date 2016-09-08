using System;

namespace DotNetRevolution.Core.GuidGeneration
{
    public class SequentialAsBytesGuidGenerator : SequentialGuidGenerator
    {
        protected override byte[] OrderBytes(byte[] timestampBytes, byte[] randomBytes)
        {
            byte[] guidBytes = new byte[16];

            // by the random data.
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
            
            return guidBytes;
        }
    }
}
