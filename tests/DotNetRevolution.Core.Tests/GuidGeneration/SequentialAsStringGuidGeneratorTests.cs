using DotNetRevolution.Core.GuidGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Core.Tests.GuidGeneration
{
    [TestClass]
    public class SequentialAsStringGuidGeneratorTests : GuidGeneratorTests
    {
        [TestInitialize]
        public void Init()
        {
            GuidGenerator = new SequentialAsStringGuidGenerator();
        }

        [TestMethod]
        public override void AssertNoClash()
        {
            base.AssertNoClash();
        }
    }
}