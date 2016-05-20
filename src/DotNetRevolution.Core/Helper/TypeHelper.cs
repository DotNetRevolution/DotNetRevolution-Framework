using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Helper
{
    public static class TypeHelper
    {
        private static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>();

        public static Type Find(string typeFullName)
        {
            Contract.Requires(typeFullName != null);

            Type type;
            
            if (Types.TryGetValue(typeFullName, out type))
            {
                return type;
            }

            lock (Types)
            {
                if (Types.TryGetValue(typeFullName, out type))
                {
                    return type;
                }

                type = Type.GetType(typeFullName) ??
                       AppDomain.CurrentDomain.GetAssemblies()
                                .Select(a => a.GetType(typeFullName))
                                .FirstOrDefault(t => t != null);

                Types.Add(typeFullName, type);

                return type;
            }
        }
    }
}
