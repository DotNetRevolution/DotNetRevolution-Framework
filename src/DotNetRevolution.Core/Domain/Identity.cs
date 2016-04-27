using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class Identity : ValueObject<Identity>
    {
        public Guid Value { get; }
                
        public Identity(Guid value)
        {
            Contract.Requires(value != Guid.Empty);

            Value = value;
        }

        public static Identity New()
        {
            Contract.Ensures(Contract.Result<Identity>() != null);

            return new Identity(SequentialGuid.Create());
        }
    }
}
