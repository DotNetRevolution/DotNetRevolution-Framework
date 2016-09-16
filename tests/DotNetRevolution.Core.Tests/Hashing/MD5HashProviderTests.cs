using DotNetRevolution.Core.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace DotNetRevolution.Core.Tests.Hashing
{
    [TestClass]
    public class MD5HashProviderTests : HashProviderTests
    {
        [TestInitialize]
        public void Init()
        {
            HashProvider = new MD5HashProvider(Encoding.UTF8);
        }

        [TestMethod]
        public override void CanGetHash()
        {
            base.CanGetHash();
        }

        internal override void AssertHashSize(byte[] hash)
        {
            Assert.IsTrue(hash.Length == 16);
        }
    }
}
