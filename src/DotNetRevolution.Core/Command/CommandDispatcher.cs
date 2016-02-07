using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory _handlerFactory;

        public CommandDispatcher(ICommandHandlerFactory handlerFactory)
        {
            Contract.Requires(handlerFactory != null);

            _handlerFactory = handlerFactory;
        }
  
        public void Dispatch(object command)
        {
            ICommandHandler handler = GetHandler(command);
            HandleCommand(command, handler);            
        }

        private ICommandHandler GetHandler(object command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            try
            {
                // get handler from factory
                return _handlerFactory.GetHandler(command.GetType());
            }
            catch (Exception e)
            {
                // rethrow exception has a command handling exception
                throw new CommandHandlingException(command, e, "Could not get a command handler for command.");
            }
        }

        private static void HandleCommand(object command, ICommandHandler handler)
        {
            Contract.Requires(handler != null);
            Contract.Requires(command != null);

            try
            {
                // handle command
                handler.Handle(command);
            }
            catch (Exception e)
            {
                // re-throw exception as a command handling exception
                throw new CommandHandlingException(command, e, "Exception occurred in command handler, check inner exception for details.");
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlerFactory != null);
        }
    }
}
