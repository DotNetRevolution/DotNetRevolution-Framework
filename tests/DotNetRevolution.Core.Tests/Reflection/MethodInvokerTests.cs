using DotNetRevolution.Core.Reflection;
using DotNetRevolution.Core.Tests.Mock;

namespace DotNetRevolution.Core.Tests.Reflection
{    
    public abstract class MethodInvokerTests
    {
        public IMethodInvoker MethodInvoker { get; set; }

        public virtual void CanInvokeMethod()
        {
            var handler = new MethodClass();

            MethodInvoker.InvokeMethodFor(handler, new MethodClassParameter());
        }
    }
}
