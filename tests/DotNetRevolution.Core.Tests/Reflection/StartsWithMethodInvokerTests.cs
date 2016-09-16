using DotNetRevolution.Core.Reflection;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Core.Tests.Reflection
{
    [TestClass]
    public class StartsWithMethodInvokerTests : MethodInvokerTests
    {
        [TestInitialize]
        public void Init()
        {
            MethodInvoker = new StartsWithMethodInvoker<MethodClass>("On");
        }

        [TestMethod]
        public override void CanInvokeMethod()
        {
            base.CanInvokeMethod();
        }
    }
}
