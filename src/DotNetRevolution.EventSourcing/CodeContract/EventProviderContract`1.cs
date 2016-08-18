//using System;
//using System.Diagnostics.Contracts;
//using DotNetRevolution.Core.Domain;

//namespace DotNetRevolution.EventSourcing.CodeContract
//{
//    [ContractClassFor(typeof(IEventProvider<>))]
//    internal abstract class EventProviderContract<TAggregateRoot> : IEventProvider<TAggregateRoot>
//        where TAggregateRoot : class, IAggregateRoot
//    {
//        public abstract EventProviderDescriptor Descriptor { get; }
//        public abstract IEventStream EventStream { get; }
//        public abstract EventProviderType EventProviderType { get; }        
//        public abstract Identity GlobalIdentity { get; }

//        public abstract Identity Identity { get; }
//        public abstract EventProviderVersion Version { get; }

//        public TAggregateRoot CreateAggregateRoot()
//        {
//            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

//            throw new NotImplementedException();
//        }

//        public EventProvider<TAggregateRoot> CreateNewVersion(IDomainEventCollection domainEvents)
//        {
//            Contract.Requires(domainEvents?.AggregateRoot != null);
//            Contract.Requires(string.IsNullOrWhiteSpace(domainEvents.AggregateRoot.ToString()) == false);
//            Contract.Requires(Contract.ForAll(domainEvents, o => o != null));
//            Contract.Ensures(Contract.Result<EventProvider>() != null);

//            throw new NotImplementedException();
//        }
//    }
//}
