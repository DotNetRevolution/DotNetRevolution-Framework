using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class AggregateRootType : ValueObject<AggregateRootType>
    {
        public Type Type { get; }

        public AggregateRootType(Type aggregateRootType)
        {            
            Contract.Requires(aggregateRootType != null);

            Type = aggregateRootType;
        }
    }
}
