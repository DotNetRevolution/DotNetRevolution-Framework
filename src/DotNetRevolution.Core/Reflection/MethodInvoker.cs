using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Reflection
{
    public abstract class MethodInvoker : IMethodInvoker
    {
        protected abstract IDictionary<Type, MethodInfo> GetCachedEntries<TInstance>();

        public void InvokeMethodFor<TInstance>(TInstance instance, object parameter)
        {
            MethodInfo methodInfo;

            var entries = GetCachedEntries<TInstance>();
            Contract.Assume(entries != null);

            if (entries.TryGetValue(parameter.GetType(), out methodInfo))
            {
                Contract.Assume(methodInfo != null);

                methodInfo.Invoke(instance, new[] { parameter });
            }
        }
    }
}
