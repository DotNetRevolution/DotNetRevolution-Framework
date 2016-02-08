using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class Identity : ValueObject<Identity>
    {
        private readonly Guid _id;

        public Guid Id
        {
            get { return _id; }
        }

        private Identity(Guid id)
        {
            Contract.Requires(id != Guid.Empty);

            _id = id;
        }

        public static Identity New()
        {
            Contract.Ensures(Contract.Result<Identity>() != null);

            return new Identity(SequentialGuid.Create());
        }
    }
}
