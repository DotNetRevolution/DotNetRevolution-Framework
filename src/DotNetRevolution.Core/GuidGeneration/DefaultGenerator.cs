using System;

namespace DotNetRevolution.Core.GuidGeneration
{
    public class DefaultGenerator : IGuidGenerator
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }

        public static Guid NewGuid()
        {
            return new DefaultGenerator().Create();
        }
    }
}
