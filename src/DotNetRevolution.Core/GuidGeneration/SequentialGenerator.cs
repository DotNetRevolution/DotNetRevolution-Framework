using System;

namespace DotNetRevolution.Core.GuidGeneration
{
    public class SequentialGuidGenerator : IGuidGenerator
    {
        private readonly SequentialGuidType _sequentialGuidType;

        public SequentialGuidGenerator(SequentialGuidType sequentialGuidType)
        {
            _sequentialGuidType = sequentialGuidType;
        }

        public Guid Create()
        {
            return SequentialGuid.Create(_sequentialGuidType);
        }
    }
}
