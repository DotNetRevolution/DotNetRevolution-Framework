using System;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using DotNetRevolution.EventSourcing.Snapshotting;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventProvider))]
    internal abstract class EventProviderContract : IEventProvider
    {
        public EventProviderDescriptor Descriptor
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderDescriptor>() != null);

                throw new NotImplementedException();
            }
        }

        public EventStream DomainEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<EventStream>() != null);

                throw new NotImplementedException();
            }
        }

        public EventProviderType EventProviderType
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderType>() != null);

                throw new NotImplementedException();
            }
        }

        public Identity GlobalIdentity
        {
            get
            {
                Contract.Ensures(Contract.Result<Identity>() != null);

                throw new NotImplementedException();
            }
        }

        public Identity Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<Identity>() != null);

                throw new NotImplementedException();
            }
        }

        public EventProviderVersion Version
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

                throw new NotImplementedException();
            }
        }

        public Snapshot GetSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
