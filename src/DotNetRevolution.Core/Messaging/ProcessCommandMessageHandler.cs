using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Command;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessCommandMessageHandler : MessageHandler<ProcessCommandMessage>
    {
        private readonly ICommandDispatcher _dispatcher;

        public ProcessCommandMessageHandler(ICommandDispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            _dispatcher = dispatcher;
        }

        public override void Handle(ProcessCommandMessage message, string correlationId)
        {
            Contract.Requires(message.Command != null);

            _dispatcher.Dispatch(message.Command);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}