using DotNetRevolution.Core.GuidGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Core.Tests.GuidGeneration
{
    [TestClass]
    public class SequentialAtEndGuidGeneratorTests : GuidGeneratorTests
    {
        [TestInitialize]
        public void Init()
        {
            GuidGenerator = new SequentialAtEndGuidGenerator();
        }

        [TestMethod]
        public override void AssertNoClash()
        {
            base.AssertNoClash();
        }
    }
}