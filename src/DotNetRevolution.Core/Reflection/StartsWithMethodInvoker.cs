using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace DotNetRevolution.Core.Reflection
{
    public class StartsWithMethodInvoker : MethodInvoker
    {
        private StartsWithMethodInvoker()
        {
        }

        public static IMethodInvoker CreateFor<T>(string methodName)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(methodName) == false);
            Contract.Ensures(Contract.Result<IMethodInvoker>() != null);

            AddToCacheIfMissing<T>(methodName);

            return new StartsWithMethodInvoker();
        }

        private static void AddToCacheIfMissing<T>(string methodName)
        {
            if (Cache<T>.Entries == null)
            {
                lock (Cache<T>.Entries)
                {
                    if (Cache<T>.Entries == null)
                    {
                        Cache<T>.Entries = typeof(T)
                            .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                            .Where(m => m.Name.StartsWith(methodName))
                            .Where(m => m.GetParameters().Length == 1)
                            .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
                    }
                }
            }
        }
    }
}
