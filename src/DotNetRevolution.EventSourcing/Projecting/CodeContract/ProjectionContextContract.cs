using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Metadata;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionContext))]
    internal abstract class ProjectionContextContract : IProjectionContext
    {
        public ICommand Command
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                throw new NotImplementedException();
            }
        }

        public object Data
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);

                throw new NotImplementedException();
            }
        }

        public IEventProvider EventProvider
        {
            get
            {
                Contract.Ensures(Contract.Result<IEventProvider>() != null);

                throw new NotImplementedException();
            }
        }

        public IReadOnlyCollection<Meta> Metadata
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<Meta>>() != null);

                throw new NotImplementedException();
            }
        }

        public TransactionIdentity TransactionIdentity
        {
            get
            {
                Contract.Ensures(Contract.Result<TransactionIdentity>() != null);

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
    }
}
