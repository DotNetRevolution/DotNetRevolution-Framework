using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandCatalog _catalog;

        public CommandDispatcher(ICommandCatalog catalog)
        {
            Contract.Requires(catalog != null);

            _catalog = catalog;
        }
  
        public void Dispatch(object command)
        {
            try
            {
                // get entry
                var entry = _catalog[command.GetType()];
                Contract.Assume(entry != null);
                
                // get a command handler
                var handler = GetHandler(entry);

                // handle command
                handler.Handle(command);
            }
            catch (Exception e)
            {
                // re-throw exception as a command handling exception
                throw new CommandHandlingException(command, e, "Exception occurred in command handler, check inner exception for details.");
            }
        }

        protected virtual ICommandHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            return (ICommandHandler) Activator.CreateInstance(handlerType);
        }

        private ICommandHandler GetHandler(ICommandEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            // get handler from entry
            var handler = entry.CommandHandler;
            
            // if handler is cached, return handler
            if (handler != null)
            {
                return handler;
            }

            // create handler
            handler = CreateHandler(entry.CommandHandlerType);

            // if handler is reusable, cache in entry
            if (handler.Reusable)
            {
                entry.CommandHandler = handler;
            }

            return handler;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_catalog != null);
        }
    }
}
