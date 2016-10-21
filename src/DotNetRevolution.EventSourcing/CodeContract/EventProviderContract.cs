using System;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventProvider))]
    internal abstract class EventProviderContract : IEventProvider
    {
        public AggregateRootType AggregateRootType
        {
            get
            {
                Contract.Ensures(Contract.Result<AggregateRootType>() != null);

                throw new NotImplementedException();
            }
        }

        public EventProviderIdentity EventProviderIdentity
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderIdentity>() != null);

                throw new NotImplementedException();
            }
        }

        public AggregateRootIdentity AggregateRootIdentity
        {
            get
            {
                Contract.Ensures(Contract.Result<AggregateRootIdentity>() != null);

                throw new NotImplementedException();
            }
        }        
    }
}
