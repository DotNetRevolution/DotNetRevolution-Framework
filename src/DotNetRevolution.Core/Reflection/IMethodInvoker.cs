using DotNetRevolution.Core.Reflection.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Reflection
{
    [ContractClass(typeof(MethodInvokerContract))]
    public interface IMethodInvoker
    {
        void InvokeMethodFor<TInstance>(TInstance instance, object parameter);
    }
}
