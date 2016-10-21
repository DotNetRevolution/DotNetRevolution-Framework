using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderTransaction
    {
        private readonly TransactionIdentity _identity;
        private readonly ICommand _command;
        private readonly IEventStream _eventStream;
        private readonly EventProviderDescriptor _descriptor;

        public TransactionIdentity Identity
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<TransactionIdentity>() != null);

                return _identity;
            }
        }

        public ICommand Command
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                return _command;
            }
        }
        
        public IEventStream EventStream
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<IEventStream>() != null);

                return _eventStream;
            }
        }

        public EventProviderDescriptor Descriptor
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<EventProviderDescriptor>() != null);

                return _descriptor;
            }
        }
        
        public EventProviderTransaction(ICommand command, IEventStream eventStream, IAggregateRoot aggregateRoot, TransactionIdentity identity)
        {
            Contract.Requires(command != null);
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(eventStream != null);
            Contract.Requires(eventStream.GetUncommittedRevisions() != null);
            Contract.Requires(eventStream.GetUncommittedRevisions().Count > 0);

            Contract.Assume(identity != null);

            _identity = identity;
            _command = command;
            _eventStream = eventStream;
            _descriptor = new EventProviderDescriptor(aggregateRoot);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identity != null);
            Contract.Invariant(_command != null);
            Contract.Invariant(_eventStream != null);
            Contract.Invariant(_descriptor != null);
        }
    }
}
