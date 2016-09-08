using System;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetRevolution.EventSourcing.Snapshotting;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamProcessor<TAggregateRoot, TAggregateRootState> : IEventStreamProcessor<TAggregateRoot, TAggregateRootState>
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
    {
        private readonly IAggregateRootBuilder<TAggregateRoot, TAggregateRootState> _aggregateRootBuilder;
        private readonly IAggregateRootStateBuilder<TAggregateRootState> _aggregateRootStateBuilder;

        public EventStreamProcessor(IAggregateRootBuilder<TAggregateRoot, TAggregateRootState> aggregateRootBuilder,
                                    IAggregateRootStateBuilder<TAggregateRootState> aggregateRootStateBuilder)
        {
            _aggregateRootBuilder = aggregateRootBuilder;
            _aggregateRootStateBuilder = aggregateRootStateBuilder;
        }

        public TAggregateRoot Process(IEventStream stream)
        {
            var state = CreateState(stream);

            // set stream as internal state tracker
            state.InternalStateTracker = new EventStreamStateTracker(stream);

            return _aggregateRootBuilder.Build(stream.EventProvider.Identity, state);
        }

        private TAggregateRootState CreateState(IEventStream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<TAggregateRootState>() != null);

            var snapshotRevision = stream.FirstOrDefault(x => x is SnapshotRevision) as SnapshotRevision;
            var domainEventRevisions = stream.Where(x => x is DomainEventRevision)
                                             .Cast<DomainEventRevision>()                                             
                                             .ToList();

            // check for valid stream
            if (snapshotRevision == null || domainEventRevisions.Any() == false)
            {
                throw new InvalidOperationException("Event stream does not contain a snapshot or domain event revision.");
            }

            if (domainEventRevisions.Any())
            {
                // order domain events and flatten list
                IReadOnlyCollection<IDomainEvent> domainEvents = domainEventRevisions.OrderBy(x => x.Version)
                                                       .SelectMany(x => x.DomainEvents)
                                                       .ToList()
                                                       .AsReadOnly();

                if (snapshotRevision == null)
                {
                    // domain events only
                    return _aggregateRootStateBuilder.Build(domainEvents);
                }
                
                // snapshot and domain events
                return _aggregateRootStateBuilder.Build(snapshotRevision.Snapshot.Data, domainEvents);
            }
            
            // snapshot only
            return _aggregateRootStateBuilder.Build(snapshotRevision.Snapshot.Data);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {            
            Contract.Invariant(_aggregateRootBuilder != null);
            Contract.Invariant(_aggregateRootStateBuilder != null);
        }
    }
}
