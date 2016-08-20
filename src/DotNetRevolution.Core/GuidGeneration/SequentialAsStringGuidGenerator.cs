using System;

namespace DotNetRevolution.Core.GuidGeneration
{
    public class SequentialAsStringGuidGenerator : SequentialGuidGenerator
    {
        protected override byte[] OrderBytes(byte[] timestampBytes, byte[] randomBytes)
        {
            byte[] guidBytes = new byte[16];

            // by the random data.
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

            // If formatting as a string, we have to compensate for the fact
            // that .NET regards the Data1 and Data2 block as an Int32 and an Int16,
            // respectively.  That means that it switches the order on little-endian
            // systems.  So again, we have to reverse.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(guidBytes, 0, 4);
                Array.Reverse(guidBytes, 4, 2);
            }

            return guidBytes;
        }
    }
}
