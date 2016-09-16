using DotNetRevolution.Core.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Core.Tests.Hashing
{
    public abstract class HashProviderTests
    {
        protected IHashProvider HashProvider { get; set; }
      
        public virtual void CanGetHash()
        {
            var hash = HashProvider.GetHash("someValue");

            Assert.IsNotNull(hash);

            AssertHashSize(hash);
        }

        internal abstract void AssertHashSize(byte[] hash);
    }
}
