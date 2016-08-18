//using DotNetRevolution.Core.Domain;
//using DotNetRevolution.EventSourcing.AggregateRoot;
//using System;
//using System.Diagnostics.Contracts;

//namespace DotNetRevolution.EventSourcing
//{
//    public class EventProvider<TAggregateRoot> : EventProvider, IEventProvider<TAggregateRoot>
//        where TAggregateRoot : class, IAggregateRoot
//    {
//        private readonly IAggregateRootProcessor _aggregateRootProcessor;
        
//        public EventProvider(Identity globalIdentity,
//            EventProviderType type,
//            Identity identity,
//            EventProviderVersion version,
//            EventProviderDescriptor descriptor,
//            IEventStream eventStream,
//            IAggregateRootProcessor aggregateRootProcessor)
//            : base(globalIdentity, type, identity, version, descriptor, eventStream)
//        {
//            Contract.Requires(globalIdentity != null);
//            Contract.Requires(type != null);
//            Contract.Requires(identity != null);
//            Contract.Requires(version != null);
//            Contract.Requires(descriptor != null);
//            Contract.Requires(eventStream != null);
//            Contract.Requires(aggregateRootProcessor != null);            

//            _aggregateRootProcessor = aggregateRootProcessor;
//        }
        
//        public EventProvider(Identity globalIdentity, IDomainEventCollection domainEventCollection, IAggregateRootProcessor aggregateRootProcessor)
//            : this(globalIdentity, 
//                   new EventProviderType(domainEventCollection.AggregateRoot.GetType()),
//                   domainEventCollection.AggregateRoot.Identity,
//                   EventProviderVersion.Initial,
//                   new EventProviderDescriptor(domainEventCollection.AggregateRoot.ToString()),
//                   new EventStream(domainEventCollection),
//                   aggregateRootProcessor)
//        {
//            Contract.Requires(globalIdentity != null);
//            Contract.Requires(domainEventCollection?.AggregateRoot != null);
//            Contract.Requires(string.IsNullOrWhiteSpace(domainEventCollection.AggregateRoot.ToString()) == false);
//            Contract.Requires(Contract.ForAll(domainEventCollection, o => o != null));
//            Contract.Requires(aggregateRootProcessor != null);            
//        }
                
//        public TAggregateRoot CreateAggregateRoot()
//        {
//            Contract.Assume(Contract.ForAll(EventStream, o => o != null));
                        
//            // process events
//            return _aggregateRootProcessor.Process<TAggregateRoot>(EventStream);
//        }
        
//        public EventProvider<TAggregateRoot> CreateNewVersion(IDomainEventCollection domainEvents)
//        {
//            var aggregateRoot = domainEvents.AggregateRoot;
            
//            // make sure aggregate root type is same as this event provider type
//            if (new EventProviderType(aggregateRoot.GetType()) != EventProviderType)
//            {                
//                throw new InvalidOperationException("Event provider type and aggregate root type do not match.");
//            }

//            // make sure aggregate root identity is the same as this event provider identity
//            if (aggregateRoot.Identity != Identity)
//            {
//                throw new InvalidOperationException("Event provider identity and aggregate root identity do not match.");
//            }
            
//            // return new event provider with incremented version
//            return new EventProvider<TAggregateRoot>(GlobalIdentity,
//                EventProviderType,
//                Identity,
//                Version.Increment(),
//                new EventProviderDescriptor(aggregateRoot.ToString()),
//                EventStream.Append(domainEvents),
//                _aggregateRootProcessor);
//        }        

//        [ContractInvariantMethod]
//        private void ObjectInvariants()
//        {
//            Contract.Invariant(_aggregateRootProcessor != null);
//        }
//    }
//}
