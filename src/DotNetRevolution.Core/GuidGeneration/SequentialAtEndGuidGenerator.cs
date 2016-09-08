using System;

namespace DotNetRevolution.Core.GuidGeneration
{
    public class SequentialAtEndGuidGenerator : SequentialGuidGenerator
    {
        protected override byte[] OrderBytes(byte[] timestampBytes, byte[] randomBytes)
        {
            byte[] guidBytes = new byte[16];

            // For sequential-at-the-end versions, we copy the random data first,
            // followed by the timestamp.
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);

            return guidBytes;
        }
    }
}
