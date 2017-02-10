using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public abstract class Projection : IProjection
    {
        public void Project(IProjectionContext context)
        {
            var genericContextType = CreateGenericType(typeof(IProjectionContext<>), context.Data.GetType());
            
            var genericContext = Activator.CreateInstance(CreateGenericType(typeof(ProjectionContext<>), context.Data.GetType()), new object[] { context.Data, context });

            foreach (var m in GetType().GetMethods())
            {
                if (m.Name == "Project")
                {
                    foreach(var p in m.GetParameters())
                    {
                        Contract.Assume(p != null);

                        if (p.ParameterType == genericContextType)
                        {
                            m.Invoke(this, new object[] { genericContext });
                            return;
                        }
                    }
                }
            }
        }

        private static Type CreateGenericType(Type typeToMake, Type typeArg)
        {
            Contract.Requires(typeToMake != null);
            Contract.Assume(typeToMake.IsGenericTypeDefinition);

            var types = new Type[] { typeArg };
            Contract.Assume(types.Length == typeToMake.GetGenericArguments().Length);

            var genericType = typeToMake.MakeGenericType(types);
            return genericType;
        }
    }  
}
