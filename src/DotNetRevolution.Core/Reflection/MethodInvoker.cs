using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Reflection
{
    public abstract class MethodInvoker : IMethodInvoker
    {
        protected static class Cache<TCache>
        {
            public static IDictionary<Type, MethodInfo> Entries;

            public static object Lock = new object();
        }
        
        public void InvokeMethodFor<TInstance>(TInstance instance, object parameter)
        {
            MethodInfo methodInfo;

            if (Cache<TInstance>.Entries.TryGetValue(parameter.GetType(), out methodInfo))
            {
                Contract.Assume(methodInfo != null);

                methodInfo.Invoke(instance, new[] { parameter });
            }
        }
    }
}
