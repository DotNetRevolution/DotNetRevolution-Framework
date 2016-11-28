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
            Contract.Requires(aggregateRootBuilder != null);
            Contract.Requires(aggregateRootStateBuilder != null);

            _aggregateRootBuilder = aggregateRootBuilder;
            _aggregateRootStateBuilder = aggregateRootStateBuilder;
        }

        public TAggregateRoot Process(IEventStream stream)
        {
            var state = CreateState(stream);
            
            // create aggregate root with state
            var aggregateRoot = _aggregateRootBuilder.Build(stream.EventProvider.AggregateRootIdentity, state);
            Contract.Assume(aggregateRoot?.State != null);

            return aggregateRoot;
        }

        private TAggregateRootState CreateState(IEventStream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<TAggregateRootState>() != null);

            var snapshotRevision = stream.FirstOrDefault(x => x is SnapshotRevision) as SnapshotRevision;

            // order domain events and flatten list
            IReadOnlyCollection<IDomainEvent> domainEvents = stream.Where(x => x is DomainEventRevision)
                                                                   .OrderBy(x => x.Version)                                     
                                                                   .Cast<DomainEventRevision>()                                                                                  
                                                                   .SelectMany(x => x.DomainEvents)
                                                                   .ToList();

            var domainEventsExist = domainEvents.Any();

            TAggregateRootState state;

            // check for valid stream
            if (snapshotRevision == null)
            {
                if (domainEventsExist)
                {
                    // domain events only
                    state = _aggregateRootStateBuilder.Build(domainEvents);
                }
                else
                {
                    throw new InvalidOperationException("Event stream does not contain a snapshot or domain event revision.");
                }
            }
            else
            {
                if (domainEventsExist)
                {
                    // snapshot and domain events
                    state = _aggregateRootStateBuilder.Build(snapshotRevision.Snapshot.Data, domainEvents);
                }
                else
                {
                    // snapshot only
                    state = _aggregateRootStateBuilder.Build(snapshotRevision.Snapshot.Data);
                }
            }
            
            Contract.Assume(state != null);
            return state;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {            
            Contract.Invariant(_aggregateRootBuilder != null);
            Contract.Invariant(_aggregateRootStateBuilder != null);
        }
    }
}
