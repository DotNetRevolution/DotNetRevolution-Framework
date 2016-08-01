
namespace DotNetRevolution.Core.GuidGeneration
{
    public static class GuidGenerator
    {
        public static IGuidGenerator Default = new DefaultGenerator();
    }
}
