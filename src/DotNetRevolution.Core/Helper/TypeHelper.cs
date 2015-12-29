using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Helper
{
    public static class TypeHelper
    {
        private static readonly Dictionary<string, Type> Types;

        static TypeHelper()
        {
            Types = new Dictionary<string, Type>();   
        }

        public static Type Find(string typeFullName)
        {
            Contract.Requires(typeFullName != null);

            lock (Types)
            {
                Type type;

                if (Types.TryGetValue(typeFullName, out type))
                {
                    return type;
                }

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetTypes().FirstOrDefault(x => x.FullName == typeFullName);

                    if (type == null)
                    {
                        continue;
                    }

                    Types[typeFullName] = type;
                    return type;
                }

                return null;
            }
        }
    }
}