using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace DotNetRevolution.Core.Reflection
{
    public class NamedMethodInvoker<T> : MethodInvoker
    {
        public NamedMethodInvoker(string methodName)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(methodName) == false);

            AddToCacheIfMissing<T>(methodName);
        }
        
        private static void AddToCacheIfMissing<TT>(string methodName)
        {         
            // check cache for entries of TT   
            if (Cache<TT>.Entries == null)
            {
                // no entries, lock cache for TT
                lock (Cache<TT>.Lock)
                {
                    // check to see if another thread beat this thread
                    if (Cache<TT>.Entries == null)
                    {
                        // no thread beat this thread, create dictionary
                        Cache<TT>.Entries = typeof(T)
                            .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                            .Where(m => m.Name == methodName)
                            .Where(m => m.GetParameters().Length == 1)
                            .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
                    }
                }
            }
        }
    }
}
