using DotNetRevolution.EventSourcing.Snapshotting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionDispatcher : IProjectionDispatcher
    {
        private readonly IProjectionManagerFactory _projectionManagerFactory;

        public ProjectionDispatcher(IProjectionManagerFactory projectionManagerFactory)
        {
            Contract.Requires(projectionManagerFactory != null);

            _projectionManagerFactory = projectionManagerFactory;
        }

        public void Dispatch(EventProviderTransaction eventProviderTransaction)
        {
            // find projection managers from aggregate root type
            var managers = _projectionManagerFactory.GetManagers(eventProviderTransaction.EventProvider.AggregateRootType);

            // check if any managers exist
            if (managers.Any())
            {
                // create projection contexts grouped by event provider and ordered by revision
                var eventProviderContexts = CreateProjectionContext(eventProviderTransaction).ToArray();

                SendContextsToProjectionManagers(eventProviderTransaction.EventProvider, managers, eventProviderContexts);
            }
        }

        public void Dispatch(params EventProviderTransaction[] eventProviderTransactions)
        {
            // group transactions by aggregate root type
            var groupedTransactionsByAggregateRootType = eventProviderTransactions.GroupBy(x => x.EventProvider.AggregateRootType);

            // go through each group and dispatch to managers
            foreach (var aggregateRootGroup in groupedTransactionsByAggregateRootType)
            {
                Contract.Assume(aggregateRootGroup?.Key != null);

                // find projection managers from aggregate root type
                var managers = _projectionManagerFactory.GetManagers(aggregateRootGroup.Key);

                // check if any managers exist
                if (managers.Any())
                {
                    // managers came back, group by event provider
                    var groupedEventProviderTransactions = aggregateRootGroup.GroupBy(x => x.EventProvider);

                    // go through each group creating projection contexts and sending contexts to managers
                    foreach (var eventProviderGroup in groupedEventProviderTransactions)
                    {
                        Contract.Assume(eventProviderGroup?.Key != null);

                        // create projection contexts grouped by event provider and ordered by revision
                        var eventProviderContexts = eventProviderGroup.OrderBy(x => x.Revisions.First().Version)
                            .Select(eventProviderTransaction => CreateProjectionContext(eventProviderTransaction))
                            .Aggregate(new List<IProjectionContext>(), (seed, transactionContexts) =>
                            {
                                seed.AddRange(transactionContexts);
                                return seed;
                            });
                        Contract.Assume(eventProviderContexts != null);

                        SendContextsToProjectionManagers(eventProviderGroup.Key, managers, eventProviderContexts.ToArray());
                    }
                }
            }
        }

        private IEnumerable<IProjectionContext> CreateProjectionContext(EventProviderTransaction eventProviderTransaction)
        {
            Contract.Requires(eventProviderTransaction != null);
            Contract.Ensures(Contract.Result<IEnumerable<IProjectionContext>>() != null);

            var contexts = new Collection<IProjectionContext>();

            // go through each revision in the transaction
            foreach(var revision in eventProviderTransaction.Revisions.OrderBy(x => x.Version))
            {
                Contract.Assume(revision != null);

                // check for domain event revision
                var domainEventRevision = revision as DomainEventRevision;

                if (domainEventRevision == null)
                {
                    // must be a snapshot revision
                    var snapshotRevision = revision as SnapshotRevision;
                    Contract.Assume(snapshotRevision != null);

                    // convert snapshot revision to snapshot projection context
                    contexts.Add(new ProjectionContext(eventProviderTransaction.EventProvider,
                        eventProviderTransaction.Identity,
                        eventProviderTransaction.Command, 
                        eventProviderTransaction.Metadata, 
                        revision.Version,
                        snapshotRevision.Snapshot));
                }

                Contract.Assume(domainEventRevision != null);

                // revision is domain event revision, go through each domain event and create a projection context
                foreach (var domainEvent in domainEventRevision.DomainEvents)
                {
                    Contract.Assume(domainEvent != null);

                    contexts.Add(new ProjectionContext(eventProviderTransaction.EventProvider,
                        eventProviderTransaction.Identity,
                        eventProviderTransaction.Command,
                        eventProviderTransaction.Metadata,
                        revision.Version,
                        domainEvent));
                }                
            }
                        
            return contexts;
        }

        private static void SendContextsToProjectionManagers(IEventProvider eventProvider, ICollection<IProjectionManager> managers, IProjectionContext[] eventProviderContexts)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(managers != null);
            Contract.Requires(eventProviderContexts != null);
            Contract.Assume(Contract.ForAll(managers, o => o != null));

            // pass contexts as a batch to each manager to handle
            foreach (var manager in managers)
            {
                Contract.Assume(manager != null);

                manager.Handle(eventProvider, eventProviderContexts);
            }
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionManagerFactory != null);
        }
    }
}
