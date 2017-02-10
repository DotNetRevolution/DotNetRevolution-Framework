using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class AggregateRootType : ValueObject<AggregateRootType>
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

        public AggregateRootType(Type aggregateRootType)
        {            
            Contract.Requires(aggregateRootType != null);

            _type = aggregateRootType;
        }

        public static implicit operator Type(AggregateRootType aggregateRootType)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(Contract.Result<Type>() != null);
            
            return aggregateRootType.Type;
        }

        public static implicit operator AggregateRootType(Type type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<AggregateRootType>() != null);

            return new AggregateRootType(type);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_type != null);
        }
    }
}
