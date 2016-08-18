using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStream))]
    internal abstract class EventStreamContract : IEventStream
    {
        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEvent>>() != null);

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

        public Snapshot Snapshot
        {
            get
            {
                Contract.Ensures(Contract.Result<Snapshot>() != null);

                throw new NotImplementedException();
            }
        }

        public IEventStream Append(IDomainEventCollection domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Ensures(Contract.Result<IEventStream>() != null);

            throw new NotImplementedException();
        }

        public abstract IEnumerator<EventStreamRevision> GetEnumerator();

        public IReadOnlyCollection<EventStreamRevision> GetUncommittedRevisions()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
