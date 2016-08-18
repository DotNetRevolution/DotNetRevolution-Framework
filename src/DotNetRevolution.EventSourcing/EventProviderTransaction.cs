using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderTransaction
    {
        private readonly Identity _identity;
        private readonly ICommand _command;
        private readonly IEventStream _eventStream;

        public Identity Identity
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<Identity>() != null);

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

        public EventProviderTransaction(ICommand command, IEventStream eventStream)
            : this(command, eventStream, Identity.New())
        {
            Contract.Requires(command != null);
            Contract.Requires(eventStream != null);
            Contract.Requires(eventStream.GetUncommittedRevisions() != null);
            Contract.Requires(eventStream.GetUncommittedRevisions().Count > 0);
        }

        public EventProviderTransaction(ICommand command, IEventStream eventStream, Identity identity)
        {
            Contract.Requires(command != null);
            Contract.Requires(eventStream != null);
            Contract.Requires(eventStream.GetUncommittedRevisions() != null);
            Contract.Requires(eventStream.GetUncommittedRevisions().Count > 0);
            Contract.Requires(identity != null);

            _identity = identity;
            _command = command;
            _eventStream = eventStream;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identity != null);
            Contract.Invariant(_command != null);
            Contract.Invariant(_eventStream != null);
        }
    }
}
