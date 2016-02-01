using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryEntry))]
    internal abstract class QueryEntryContract : IQueryEntry
    {
        public Type QueryType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public Type QueryHandlerType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract IQueryHandler QueryHandler { get; set; }
    }
}
