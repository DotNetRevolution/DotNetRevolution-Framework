using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Reflection.CodeContract
{
    [ContractClassFor(typeof(IMethodInvoker))]
    internal abstract class MethodInvokerContract : IMethodInvoker
    {
        public void InvokeMethodFor<TInstance>(TInstance instance, object parameter)
        {
            Contract.Requires(instance != null);
            Contract.Requires(parameter != null);
        }
    }
}
