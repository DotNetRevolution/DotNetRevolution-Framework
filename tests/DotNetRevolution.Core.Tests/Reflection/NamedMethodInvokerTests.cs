using DotNetRevolution.Core.Reflection;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Core.Tests.Reflection
{
    [TestClass]
    public class NamedMethodInvokerTests : MethodInvokerTests
    {
        [TestInitialize]
        public void Init()
        {
            MethodInvoker = new NamedMethodInvoker<MethodClass>("When");
        }

        [TestMethod]        
        public override void CanInvokeMethod()
        {
            base.CanInvokeMethod();            
        }
    }
}
