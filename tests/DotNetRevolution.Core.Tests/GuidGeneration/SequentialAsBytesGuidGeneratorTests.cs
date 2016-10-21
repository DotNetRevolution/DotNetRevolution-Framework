using DotNetRevolution.Core.GuidGeneration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Core.Tests.GuidGeneration
{
    [TestClass]
    public class SequentialAsBytesGuidGeneratorTests : GuidGeneratorTests
    {
        [TestInitialize]
        public void Init()
        {
            GuidGenerator = new SequentialAsBytesGuidGenerator();
        }

        [TestMethod]
        public override void AssertNoClash()
        {
            base.AssertNoClash();
        }
    }
}