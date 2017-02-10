using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionType : ValueObject<ProjectionType>
    {
        private readonly Type _type;

        public Type Type
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                return _type;
            }
        }

        public ProjectionType(Type aggregateRootType)
        {
            Contract.Requires(aggregateRootType != null);

            _type = aggregateRootType;
        }

        public static implicit operator Type(ProjectionType projectionType)
        {
            Contract.Requires(projectionType != null);
            Contract.Ensures(Contract.Result<Type>() != null);

            return projectionType.Type;
        }

        public static implicit operator ProjectionType(Type type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<ProjectionType>() != null);

            return new ProjectionType(type);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_type != null);
        }
    }
}
