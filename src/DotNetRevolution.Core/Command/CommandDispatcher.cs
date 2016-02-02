using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public CommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
        {
            Contract.Requires(commandHandlerFactory != null);

            _commandHandlerFactory = commandHandlerFactory;
        }
  
        public void Dispatch(object command)
        {
            ICommandHandler handler = GetHandler(command);
            HandleCommand(handler, command);            
        }

        private ICommandHandler GetHandler(object command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            try
            {
                // get handler from factory
                return _commandHandlerFactory.Get(command.GetType());
            }
            catch (Exception e)
            {
                // rethrow exception has a command handling exception
                throw new CommandHandlingException(command, e, "Could not get a command handler for command.");
            }
        }

        private static void HandleCommand(ICommandHandler handler, object command)
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
            Contract.Invariant(_commandHandlerFactory != null);
        }
    }
}
